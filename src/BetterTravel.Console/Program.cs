using System.Threading.Tasks;
using BetterTravel.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PuppeteerSharp;

namespace BetterTravel.Console
{
    public class Program
    {
        private static ILogger<Program> logger;

        private static async Task Main()
        {
            var serviceProvider = new Startup().ConfigureServices();
            logger = serviceProvider
                .GetService<ILoggerFactory>()
                .CreateLogger<Program>();
            
            await FetchBrowser();

            var testService = serviceProvider.GetService<ITestService>();
            await testService.RunTestAsync();
        }

        private static async Task FetchBrowser()
        {
            logger.LogInformation("Fetching browser...");
            await new BrowserFetcher().DownloadAsync(BrowserFetcher.DefaultRevision);
            logger.LogInformation("Browser fetched successfully!");
        }
    }
}