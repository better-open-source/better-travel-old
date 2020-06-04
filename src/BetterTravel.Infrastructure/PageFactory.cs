using System.Threading.Tasks;
using PuppeteerSharp;
using Serilog;
using ILogger = Serilog.ILogger;

namespace BetterTravel.Infrastructure
{
    public interface IBrowserPageFactory
    {
        Task<Page> ConcretePageAsync(bool withCookies);
    }
    
    public class BrowserPageFactory : IBrowserPageFactory
    {
        private readonly ILogger _logger;
        private readonly CookieParam[] _cookies;

        public BrowserPageFactory(CookieParam[] cookies) => 
            (_logger, _cookies) = (Log.ForContext<BrowserPageFactory>(), cookies);

        public async Task<Page> ConcretePageAsync(bool withCookies)
        {
            var browser = await InitBrowserAsync();
            var page = await browser.NewPageAsync();
            if (withCookies)
                await page.SetCookieAsync(_cookies);
            
            _logger.Information($"Created new browser. Cookies: {withCookies.ToString()}", page.Url);
            return page;
        }
        
        private static async Task<Browser> InitBrowserAsync() => 
            await Puppeteer.LaunchAsync(new LaunchOptions {Headless = true, Args = new []{"--no-sandbox"}});
    }

}