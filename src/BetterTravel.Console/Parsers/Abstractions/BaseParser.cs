using System;
using System.Threading.Tasks;
using BetterTravel.Console.Common;
using PuppeteerSharp;

namespace BetterTravel.Console.Parsers.Abstractions
{
    public abstract class BaseParser : IDisposable
    {
        private readonly Browser _browser;

        protected BaseParser() =>
            _browser = InitBrowserAsync()
                .ConfigureAwait(false)
                .GetAwaiter()
                .GetResult();

        private static async Task<Browser> InitBrowserAsync() => 
            await Puppeteer.LaunchAsync(new LaunchOptions {Headless = false});

        protected async Task<Page> GetNewPage()
        {
            var page = await _browser.NewPageAsync();
            await page.GoToAsync("https://www.instagram.com/accounts/login/");
            await page.WaitForTimeoutAsync(2500);
            await page.WaitForSelectorAsync(Consts.UserNameSelector);
            await page.FocusAsync(Consts.UserNameSelector);
            await page.Keyboard.TypeAsync(Consts.InstagramUesrname);
            await page.WaitForSelectorAsync(Consts.PasswordSelector);
            await page.FocusAsync(Consts.PasswordSelector);
            await page.Keyboard.TypeAsync(Consts.InstagramPassword);
            await page.WaitForSelectorAsync(Consts.SubmitBtnSelector);
            await page.ClickAsync(Consts.SubmitBtnSelector);
            await page.WaitForTimeoutAsync(2500);
            return page;
        }

        public void Dispose()
        {
            _browser.CloseAsync()
                .ConfigureAwait(false)
                .GetAwaiter()
                .GetResult();
        }
    }
}