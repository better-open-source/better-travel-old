using System.Diagnostics;
using System.Threading.Tasks;
using BetterTravel.Common;
using PuppeteerSharp;

namespace BetterTravel.Services
{
    public interface IAuthService
    {
        Task<CookieParam[]> AuthenticateAsync(string username, string password, int timeout);
    }
    
    public class AuthService : IAuthService
    {
        private readonly Browser _browser;

        public AuthService(Browser browser) => 
            _browser = browser;

        public async Task<CookieParam[]> AuthenticateAsync(string username, string password, int timeout)
        {
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
            
            Debugger.Log(0, nameof(AuthService), "Logged is successfully...");
            
            return await page.GetCookiesAsync();
        }
    }
}