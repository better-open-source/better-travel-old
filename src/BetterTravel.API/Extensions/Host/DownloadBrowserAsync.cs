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
            if (string.IsNullOrEmpty(Puppeteer.GetExecutablePath()))
            {
                Log.Warning("Puppeteer browser path is null or empty");
                await new BrowserFetcher().DownloadAsync(BrowserFetcher.DefaultRevision);
            }
            else
            {
                Log.Information($"Puppeteer browser path: {Puppeteer.GetExecutablePath()}");
            }
            
            return host;
        }
    }
}