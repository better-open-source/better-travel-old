using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using PuppeteerSharp;
using Serilog;

namespace BetterTravel.API.Extensions.Host
{
    public static partial class HostExtensions
    {
        public static async Task<IHost> DownloadBrowserAsync(this IHost host)
        {
            await new BrowserFetcher().DownloadAsync(BrowserFetcher.DefaultRevision);
            return host;
        }
    }
}