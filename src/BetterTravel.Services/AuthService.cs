using System;
using System.Linq;
using System.Threading.Tasks;
using BetterTravel.Common;
using BetterTravel.Common.Configurations;
using PuppeteerSharp;
using Serilog;
using ILogger = Serilog.ILogger;

namespace BetterTravel.Services
{
    public interface IAuthService
    {
        Task<CookieParam[]> AuthenticateAsync(string username, string password, int timeout);
    }

    public class AuthService : IAuthService, IDisposable
    {
        private readonly ILogger _logger;
        private readonly Browser _browser;

        public AuthService() => 
            (_logger, _browser) = (Log.ForContext<AuthService>(), InitBrowser());

        public async Task<CookieParam[]> AuthenticateAsync(string username, string password, int timeout)
        {
            _logger.Information(
                "Try to authenticate...", username, new string(password.Select(t => '*').ToArray()));
            
            var page = await _browser.NewPageAsync();
            await page.GoToAsync("https://www.instagram.com/accounts/login/");
            await page.WaitForTimeoutAsync(timeout);

            await page.WaitForSelectorAsync(Consts.UserNameSelector);
            await page.FocusAsync(Consts.UserNameSelector);
            await page.Keyboard.TypeAsync(InstagramConfiguration.Username);

            await page.WaitForSelectorAsync(Consts.PasswordSelector);
            await page.FocusAsync(Consts.PasswordSelector);
            await page.Keyboard.TypeAsync(InstagramConfiguration.Password);
            await page.WaitForSelectorAsync(Consts.SubmitBtnSelector);

            await page.ClickAsync(Consts.SubmitBtnSelector);
            await page.WaitForTimeoutAsync(timeout);

            _logger.Information("Successfully authenticated!");
            return await page.GetCookiesAsync();
        }

        public void Dispose() => 
            _browser.Dispose();
        
        private static Browser InitBrowser() =>
            Puppeteer.LaunchAsync(new LaunchOptions {Headless = true, Args = new []{"--no-sandbox"}})
                .ConfigureAwait(false).GetAwaiter().GetResult();
    }
}