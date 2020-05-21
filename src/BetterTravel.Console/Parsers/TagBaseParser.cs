using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BetterTravel.Console.Common;
using BetterTravel.Console.Domain;
using BetterTravel.Console.Parsers.Abstractions;
using Fizzler.Systems.HtmlAgilityPack;
using HtmlAgilityPack;
using PuppeteerSharp;

namespace BetterTravel.Console.Parsers
{
    public class TagBaseParser : BaseParser, ITagParser
    {
        private readonly Page _page;

        public TagBaseParser(string tag)
        {
            var hashTagUrl = $"{Consts.BaseUrl}/explore/tags/{tag}/?hl=en";

            _page = 
                GetNewPage()
                    .ConfigureAwait(false)
                    .GetAwaiter()
                    .GetResult();
            
            _page
                .Log($"Navigate to page: {hashTagUrl}")
                .GoToAsync(hashTagUrl)
                .ConfigureAwait(false)
                .GetAwaiter()
                .GetResult();
        }
        
        public async Task<IEnumerable<PostInfo>> GetPosts()
        {
            await _page.WaitForTimeoutAsync(2500);
            return (await GetPosts(3, _page)).ToList();
        }
        
        private static async Task<IEnumerable<PostInfo>> GetPosts(int rowCount, Page page) =>
            await Enumerable.Range(1, rowCount)
                .SelectMany(row =>
                    Enumerable
                        .Range(1, 3)
                        .Select(cell => (row, cell)))
                .Select(tuple => GetPost(page, tuple))
                .WhenAllSync();

        private static async Task<PostInfo> GetPost(Page page, (int row, int cell) tuple)
        {
            var (rowIdx, cellIdx) = tuple
                .Log($" Func {nameof(GetPost)} | row: {tuple.row} | cell: {tuple.cell}");
            var time = new Random();
            var randInt = time.Next(0, 100);

            await page.ClickAsync(
                $"{Consts.HashTagsBaseClass} > div > div > .Nnq7C:nth-child({rowIdx}) > .v1Nh3:nth-child({cellIdx}) > a");
            await page.WaitForTimeoutAsync(2500);
            
            var html = await page.GetContentAsync();
            var doc = new HtmlDocument();
            doc.LoadHtml(html);
            
            await page.WaitForTimeoutAsync(2250 + randInt);
            await page.ClickAsync(Consts.PostСloseButton);
            await page.WaitForTimeoutAsync(2250 + randInt);

            return doc.DocumentNode
                .QuerySelectorAll("div > article")
                .Select(MapPost)
                .FirstOrDefault();
        }
        
        private static PostInfo MapPost(HtmlNode articleNode)
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
                ExtractPostAuthor(articleNode));
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