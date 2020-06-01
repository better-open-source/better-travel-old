using System;
using System.Linq;
using System.Threading.Tasks;
using BetterTravel.Common;
using Microsoft.Extensions.Logging;
using PuppeteerSharp;

namespace BetterTravel.Services
{
    public interface IAuthService
    {
        Task<CookieParam[]> AuthenticateAsync(string username, string password, int timeout);
    }

    public class AuthService : IAuthService, IDisposable
    {
        private readonly ILogger<AuthService> _logger;
        private readonly Browser _browser;

        public AuthService(ILogger<AuthService> logger) => 
            (_logger, _browser) = (logger, InitBrowser());

        public async Task<CookieParam[]> AuthenticateAsync(string username, string password, int timeout)
        {
            _logger.LogInformation(
                $"Try to authenticate...", username, new string(password.Select(t => '*').ToArray()));
            
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

            _logger.LogInformation("Successfully authenticated!");
            return await page.GetCookiesAsync();
        }

        public void Dispose() => 
            _browser.Dispose();
        
        private static Browser InitBrowser() =>
            Puppeteer.LaunchAsync(new LaunchOptions {Headless = true})
                .ConfigureAwait(false).GetAwaiter().GetResult();
    }
}