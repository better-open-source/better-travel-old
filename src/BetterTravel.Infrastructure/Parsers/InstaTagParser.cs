using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BetterTravel.Common;
using BetterTravel.Infrastructure.Domain;
using BetterTravel.Infrastructure.Parsers.Abstractions;
using Fizzler.Systems.HtmlAgilityPack;
using HtmlAgilityPack;

namespace BetterTravel.Infrastructure.Parsers
{
    public class InstaTagParser : BaseParser<InstaTagParser>, ITagParser
    {
        public InstaTagParser(string tag, IBrowserPageFactory pageFactory) 
            : base(pageFactory)
        {
            var hashTagUrl = $"{Consts.BaseUrl}/explore/tags/{tag}/?hl=en";
            Logger.Information($"Navigate to page: {hashTagUrl}");
            Page.ConsoleWriteLine(string.Empty)
                .GoToAsync(hashTagUrl)
                .ConfigureAwait(false)
                .GetAwaiter()
                .GetResult();
        }

        public async Task<IEnumerable<PostInfo>> GetPostsAsync()
        {
            try
            {
                await Page.WaitForTimeoutAsync(2500);
                return (await GetPosts(3)).ToList();
            }
            catch (Exception e)
            {
                Logger.Error(e, $"{nameof(InstaTagParser)} failed for page {Page.Url}");
                return new List<PostInfo>();
            }
        }

        private async Task<IEnumerable<PostInfo>> GetPosts(int rowCount) =>
            await Enumerable.Range(1, rowCount)
                .SelectMany(row =>
                    Enumerable
                        .Range(1, 3)
                        .Select(cell => (row, cell)))
                .Select(GetPost)
                .WhenAllSync();

        private async Task<PostInfo> GetPost((int row, int cell) tuple)
        {
            Logger.Information($" Func {nameof(GetPost)} | row: {tuple.row} | cell: {tuple.cell}");
            
            var (rowIdx, cellIdx) = tuple;
            var time = new Random();
            var randInt = time.Next(0, 100);

            await Page.ClickAsync(
                $"{Consts.HashTagsBaseClass} > div > div > .Nnq7C:nth-child({rowIdx}) > .v1Nh3:nth-child({cellIdx}) > a");
            await Page.WaitForTimeoutAsync(2500);

            var html = await Page.GetContentAsync();
            var doc = new HtmlDocument();
            doc.LoadHtml(html);

            await Page.WaitForTimeoutAsync(2250 + randInt);
            await Page.ClickAsync(Consts.PostСloseButton);
            await Page.WaitForTimeoutAsync(2250 + randInt);

            return doc.DocumentNode
                .QuerySelectorAll("div > article")
                .Select(MapPost)
                .FirstOrDefault();
        }

        private static PostInfo MapPost(HtmlNode articleNode)
        {
            var node = articleNode
                .QuerySelectorAll("div > div > ul > div > li > div > div > div")
                .FirstOrDefault(n => !n
                    .GetAttributeValue("role", string.Empty)
                    .Contains("button"));

            if (node is null)
                return null;
            
            var description = new Descritption(
                ExtractDescriptionDate(node),
                ExtractDescriptionText(node),
                ExtractDescriptionHashTags(node));
            
            var postUrl = $"https://www.instagram.com{ExtractPostUrl(articleNode)}";
            var imgUrl = $"{postUrl}media/?size=m";
            var author = ExtractPostAuthor(articleNode);

            return new PostInfo(
                description,
                imgUrl,
                author,
                postUrl
            );
        }

        private static string ExtractPostAuthor(HtmlNode node) =>
            node
                .QuerySelectorAll("header > div > div > div > a")
                .FirstOrDefault()
                ?.InnerText;

        private static string ExtractPostUrl(HtmlNode node) =>
            SelectAttributes(node, "div > div > a", "href", string.Empty)
                .FirstOrDefault(attr => attr.Contains("/p/"));

        private static string ExtractDescriptionText(HtmlNode node) =>
            node
                .QuerySelectorAll("span")
                .FirstOrDefault()?
                .InnerText;

        private static string ExtractDescriptionDate(HtmlNode node) =>
            SelectAttributes(node, "div > div > time", "title", string.Empty)
                .FirstOrDefault();

        private static IEnumerable<string> ExtractDescriptionHashTags(HtmlNode node) =>
            node
                .QuerySelectorAll("span > a")
                .Where(n => n.InnerHtml.Contains("#"))
                .Select(n => n.InnerText);

        private static IEnumerable<T> SelectAttributes<T>(HtmlNode node, string query, string attrName, T def) =>
            node
                .QuerySelectorAll(query)
                .Select(n => n.GetAttributeValue(attrName, def));
    }
}