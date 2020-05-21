using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BetterTravel.Console.Common;
using BetterTravel.Console.Domain;
using BetterTravel.Console.Parsers;
using BetterTravel.Console.Parsers.Abstractions;
using PuppeteerSharp;

namespace BetterTravel.Console
{
    internal static class Program
    {
        private static async Task Main()
        {
            await new BrowserFetcher().DownloadAsync(BrowserFetcher.DefaultRevision);

            var results = await GetAllPosts();
            results.ToList()
                .ForEach(post => System.Console.WriteLine($"{post}\n\n"));
        }

        private static async Task<IEnumerable<PostInfo>> GetAllPosts() =>
            (await
                (await Consts.HashTags
                    .Select(InitParserAsync)
                    .WhenAll())
                .Select(t => t.GetPosts())
                .WhenAll())
            .SelectMany(t => t);
        
        private static async Task<ITagParser> InitParserAsync(string tag) =>
            await Task.Factory.StartNew(() => new TagBaseParser(tag));
    }
}