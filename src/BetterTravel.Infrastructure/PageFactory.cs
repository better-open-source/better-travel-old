using System.Threading.Tasks;
using PuppeteerSharp;

namespace BetterTravel.Infrastructure
{
    public interface IBrowserPageFactory
    {
        Task<Page> ConcretePageAsync(bool withCookies);
    }
    
    public class BrowserPageFactory : IBrowserPageFactory
    {
        private readonly CookieParam[] _cookies;

        public BrowserPageFactory(CookieParam[] cookies) => 
            _cookies = cookies;

        public async Task<Page> ConcretePageAsync(bool withCookies)
        {
            var browser = await InitBrowserAsync();
            var page = await browser.NewPageAsync();
            if (withCookies)
                await page.SetCookieAsync(_cookies);
            return page;
        }
        
        private static async Task<Browser> InitBrowserAsync() => 
            await Puppeteer.LaunchAsync(new LaunchOptions {Headless = false});
    }

}