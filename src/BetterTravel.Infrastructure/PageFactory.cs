using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using PuppeteerSharp;

namespace BetterTravel.Infrastructure
{
    public interface IBrowserPageFactory
    {
        Task<Page> ConcretePageAsync(bool withCookies);
    }
    
    public class BrowserPageFactory : IBrowserPageFactory
    {
        private readonly ILogger<BrowserPageFactory> _logger;
        private readonly CookieParam[] _cookies;

        public BrowserPageFactory(ILogger<BrowserPageFactory> logger, CookieParam[] cookies) => 
            (_logger, _cookies) = (logger, cookies);

        public async Task<Page> ConcretePageAsync(bool withCookies)
        {
            var browser = await InitBrowserAsync();
            var page = await browser.NewPageAsync();
            if (withCookies)
                await page.SetCookieAsync(_cookies);

            _logger.LogInformation($"Created new browser. Cookies: {withCookies.ToString()}", page.Url);
            return page;
        }
        
        private static async Task<Browser> InitBrowserAsync() => 
            await Puppeteer.LaunchAsync(new LaunchOptions {Headless = true});
    }

}