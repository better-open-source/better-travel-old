using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Threading;
using System.Threading.Tasks;
using Fizzler.Systems.HtmlAgilityPack;
using HtmlAgilityPack;
using PuppeteerSharp;

namespace better_travel.Console
{
    internal static class Program
    {
        private static async Task Main()
        {
            await InitPuppeteerAsync();
            var browser = await InitBrowserAsync();
            var page = await browser.NewPageAsync();
            var InstagramConfig = new AccountInformation();
            await page.GoToAsync("https://www.instagram.com/accounts/login/");
            await page.WaitForTimeoutAsync(2500);
            await page.WaitForSelectorAsync(InstagramConfig.UserNameSelector);
            await page.FocusAsync(InstagramConfig.UserNameSelector);
            await page.Keyboard.TypeAsync(InstagramConfig.InstagramUesrname);
            await page.WaitForSelectorAsync(InstagramConfig.PasswordSelector);
            await page.FocusAsync(InstagramConfig.PasswordSelector);
            await page.Keyboard.TypeAsync(InstagramConfig.InstagramPassword);
            await page.WaitForSelectorAsync(InstagramConfig.SubmitBtnSelector);
            await page.ClickAsync(InstagramConfig.SubmitBtnSelector);
            var tester = await VisitHashtagUrl(InstagramConfig, page);
            await browser.CloseAsync();
        }

        private static async Task<List<List<PostInfo>>> VisitHashtagUrl(AccountInformation InstagramConfig, Page page)
        {
            var AllInfo = new List<List<PostInfo>>();

            foreach (var t in InstagramConfig.Hashtag)
            {
                await page.WaitForTimeoutAsync(2500);
                var HashtagUrl = ($"{InstagramConfig.BaseUrl}/explore/tags/{t}/?hl=en");
                await page.GoToAsync(HashtagUrl);
                await page.WaitForTimeoutAsync(2500);
                var data = await GetPost(InstagramConfig, page);
                AllInfo.Add(data);
            }

            return AllInfo;
        }

        private static async Task<List<PostInfo>> GetPost(AccountInformation InstagramConfig, Page page)
        {
            var AllInfo=new List<PostInfo>();
            for (var r = 1; r < 4; r++) //loops through each row
            {
                for (var c = 1; c < 4; c++) //loops through each item in the row
                {
                    Random time = new Random();
                    int rInt = time.Next(0, 100);
                    //Select post
                    await page.ClickAsync(
                        $"{InstagramConfig.HashTagsBaseClass} > div > div > .Nnq7C:nth-child({r}) >  .v1Nh3:nth-child({c}) > a");
                    var html = await page.GetContentAsync();
                    var doc = new HtmlDocument();
                    doc.LoadHtml(html);
                    var board = doc.DocumentNode.QuerySelectorAll("div > article").Select(MapPostInfo).ToList()
                        .FirstOrDefault();
                    AllInfo.Add(board);
                    await page.WaitForTimeoutAsync(
                        2250 + rInt); //wait for random amount of time
                    //Closing the current post modal
                    await page.ClickAsync(InstagramConfig.PostСloseButton);
                    //Wait for random amount of time
                    await page.WaitForTimeoutAsync(
                        2250 + rInt);
                }
            }

            return AllInfo;
        }

        private static PostInfo MapPostInfo(HtmlNode board)
        {
            var descritption = new Descritption();

            var descriptionInformation = board
                .QuerySelectorAll("div > div > ul > div > li > div > div > div")
                .FirstOrDefault(n => !n
                    .GetAttributeValue("role", string.Empty)
                    .Contains("button"));

            //ToDo: put all description logic in a separate method
            descritption.Text = descriptionInformation
                .QuerySelectorAll("span")
                .FirstOrDefault()?
                .InnerText;

            descritption.Date = descriptionInformation
                .QuerySelectorAll("div > div > time")
                .FirstOrDefault()
                ?.GetAttributes("title")
                .FirstOrDefault()
                ?.Value;

            //ToDo: in the future we can display hashtags statistics, and add new hashtags  for the search
            descritption.Hashtag = descriptionInformation
                .QuerySelectorAll(@"span > a")
                .Where(items => items.InnerHtml.Contains("#"));

            var imgUrl = board
                .QuerySelectorAll("div >  div > div > div > img")
                .FirstOrDefault()
                ?.GetAttributes("src")
                .FirstOrDefault()
                ?.Value.Replace("amp;", "");

            var author = board
                .QuerySelectorAll("header > div > div > div > a")
                .FirstOrDefault()
                ?.InnerText;

            return new PostInfo(descritption, imgUrl, author);
        }

        private static async Task InitPuppeteerAsync() =>
            await new BrowserFetcher().DownloadAsync(BrowserFetcher.DefaultRevision);

        private static async Task<Browser> InitBrowserAsync() =>
            await Puppeteer.LaunchAsync(new LaunchOptions
            {
                Headless = false
            });
    }
}