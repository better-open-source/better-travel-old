using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BetterTravel.Console.Domain;
using Fizzler.Systems.HtmlAgilityPack;
using HtmlAgilityPack;
using PuppeteerSharp;

namespace BetterTravel.Console
{
    internal static class Program
    {
        private static async Task Main()
        {
            await InitPuppeteerAsync();
            var browser = await InitBrowserAsync();
            var page = await browser.NewPageAsync();
            var instagramConfig = new AccountInformation();
            await page.GoToAsync("https://www.instagram.com/accounts/login/");
            await page.WaitForTimeoutAsync(2500);
            await page.WaitForSelectorAsync(instagramConfig.UserNameSelector);
            await page.FocusAsync(instagramConfig.UserNameSelector);
            await page.Keyboard.TypeAsync(instagramConfig.InstagramUesrname);
            await page.WaitForSelectorAsync(instagramConfig.PasswordSelector);
            await page.FocusAsync(instagramConfig.PasswordSelector);
            await page.Keyboard.TypeAsync(instagramConfig.InstagramPassword);
            await page.WaitForSelectorAsync(instagramConfig.SubmitBtnSelector);
            await page.ClickAsync(instagramConfig.SubmitBtnSelector);
            var tester = await VisitHashtagUrl(instagramConfig, page);
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
            var AllInfo = new List<PostInfo>();
            for (var r = 1; r < 4; r++) //loops through each row
            {
                for (var c = 1; c < 4; c++) //loops through each item in the row
                {
                    Random time = new Random();
                    int rInt = time.Next(0, 100);
                    //Select post
                    await page.ClickAsync(
                        $"{InstagramConfig.HashTagsBaseClass} > div > div > .Nnq7C:nth-child({r}) >  .v1Nh3:nth-child({c}) > a");
                    await page.WaitForTimeoutAsync(2500);
                    var html = await page.GetContentAsync();
                    var doc = new HtmlDocument();
                    doc.LoadHtml(html);
                    var board = doc.DocumentNode.QuerySelectorAll("div > article")
                        .Select(MapPostInfo)
                        .ToList();
                    AllInfo.AddRange(board);
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

        private static async Task InitPuppeteerAsync() =>
            await new BrowserFetcher().DownloadAsync(BrowserFetcher.DefaultRevision);

        private static async Task<Browser> InitBrowserAsync() =>
            await Puppeteer.LaunchAsync(new LaunchOptions
            {
                Headless = false
            });
        
        private static PostInfo MapPostInfo(HtmlNode articleNode)
        {
            var node = articleNode
                .QuerySelectorAll("div > div > ul > div > li > div > div > div")
                .FirstOrDefault(n => !n
                    .GetAttributeValue("role", string.Empty)
                    .Contains("button"));

            var description = new Descritption(
                ExtractDescriptionDate(node),
                ExtractDescriptionText(node),
                ExtractDescriptionHashTags(node));
            
            return new PostInfo(
                description, 
                ExtractPostImage(articleNode), 
                ExtractPostAuthor(node));
        }

        private static string ExtractPostImage(HtmlNode node) =>
            SelectAttributes(node, "div >  div > div > div > img", "src", string.Empty)
                .FirstOrDefault()
                ?.Replace("amp;", "");

        private static string ExtractPostAuthor(HtmlNode node) =>
            node
                .QuerySelectorAll("header > div > div > div > a")
                .FirstOrDefault()
                ?.InnerText;
        
        private static string ExtractDescriptionText(HtmlNode node) =>
            node
                .QuerySelectorAll("span")
                .FirstOrDefault()?
                .InnerText;

        private static string ExtractDescriptionDate(HtmlNode node) =>
            SelectAttributes(node, "div > div > time", "title", string.Empty)
                .FirstOrDefault();
        
        private static IEnumerable<string> ExtractDescriptionHashTags(HtmlNode node) =>
            node
                .QuerySelectorAll("span > a")
                .Where(n => n.InnerHtml.Contains("#"))
                .Select(n => n.InnerText);

        private static IEnumerable<T> SelectAttributes<T>(HtmlNode node, string query, string attrName, T def) =>
            node
                .QuerySelectorAll(query)
                .Select(n => n.GetAttributeValue(attrName, def));
    }
}