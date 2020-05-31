using System.Threading.Tasks;
using BetterTravel.Common;
using BetterTravel.DataAccess;
using BetterTravel.Infrastructure;
using BetterTravel.Services;
using PuppeteerSharp;

namespace BetterTravel.Console
{
    internal static class Program
    {
        private static async Task Main()
        {
            await FetchBrowser();
            
            var cookies = await GetCookiesAsync();

            IBrowserPageFactory pageFactory = new BrowserPageFactory(cookies);
            ITestService testService = new TestService(new AppDbContext(), pageFactory);
            
            await testService.RunTestAsync();
        }

        private static async Task FetchBrowser() => 
            await new BrowserFetcher().DownloadAsync(BrowserFetcher.DefaultRevision);

        private static async Task<CookieParam[]> GetCookiesAsync()
        {
            var browser = await Puppeteer.LaunchAsync(new LaunchOptions {Headless = true});
            IAuthService authService = new AuthService(browser);
            
            var cookies = await authService.AuthenticateAsync(
                InstagramConfiguration.Username,
                InstagramConfiguration.Password,
                2500);
            
            await browser.CloseAsync();
            return cookies;
        }
    }
}