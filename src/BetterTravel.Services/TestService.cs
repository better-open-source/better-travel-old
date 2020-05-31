﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BetterTravel.Common;
using BetterTravel.DataAccess;
using BetterTravel.Domain;
using BetterTravel.Infrastructure;
using BetterTravel.Infrastructure.Parsers;
using BetterTravel.Infrastructure.Parsers.Abstractions;

namespace BetterTravel.Services
{
    public interface ITestService
    {
        Task RunTestAsync();
    }

    public class TestService : ITestService
    {
        private readonly IBrowserPageFactory _pageFactory;


        public TestService(IBrowserPageFactory pageFactory) =>
            _pageFactory = pageFactory;

        public async Task RunTestAsync()
        {
            var results = await GetAllPosts(Consts.HashTags);
            results
                .ToList()
                .ForEach(post => System.Console.WriteLine($"{post}\n\n"))
                ;
            using (TourInfoContext db = new TourInfoContext())
            {
                results.ToList().ForEach(post => db.ToursInfo.Add( new TourInfo{ Description = post.Description , PostUrl = post.PostUrl , Author =  post.Author , ImgUrl = post.ImgUrl}));
                db.SaveChanges();
                
            }

            /*db.Instagrams.Add(user2);
            db.SaveChanges();*/
        }

        private async Task<IEnumerable<PostInfo>> GetAllPosts(IEnumerable<string> tags)
        {
            var parsers = await tags.Select(InitParser).WhenAll();
            var posts = await parsers.Select(parser => parser.GetPostsAsync()).WhenAll();
            var result = posts.SelectMany(seq => seq);
            return result;
        }

        private async Task<ITagParser> InitParser(string tag) =>
            await Task.Factory.StartNew(() => new TagParser(tag, _pageFactory));
    }
}