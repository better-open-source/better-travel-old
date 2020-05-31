using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BetterTravel.Common;
using BetterTravel.DataAccess;
using BetterTravel.DataAccess.Models;
using BetterTravel.Domain;
using BetterTravel.Infrastructure;
using BetterTravel.Infrastructure.Parsers;
using BetterTravel.Infrastructure.Parsers.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace BetterTravel.Services
{
    public interface ITestService
    {
        Task RunTestAsync();
    }

    public class TestService : ITestService
    {
        private readonly AppDbContext _dbContext;
        private readonly IBrowserPageFactory _pageFactory;

        public TestService(AppDbContext dbContext, IBrowserPageFactory pageFactory) => 
            (_dbContext, _pageFactory) = (dbContext, pageFactory);

        public async Task RunTestAsync()
        {
            var results = await GetAllPosts(Consts.HashTags);
            var tours = results
                .Where(t => !(t is null))
                .Select(MapTourInfo);

            var t1 = await _dbContext.ToursInfo.ToListAsync();
            await _dbContext.ToursInfo.AddRangeAsync(tours);
            await _dbContext.SaveChangesAsync();
            
            var t2 = await _dbContext.ToursInfo.ToListAsync();
        }

        private static TourInfo MapTourInfo(PostInfo post) =>
            new TourInfo
                { 
                    PostUrl = post.PostUrl, 
                    Author = post.Author, 
                    ImgUrl = post.ImgUrl,
                    Date = post.Description.Date,
                    HashTags = string.Join(", ", post.Description.HashTags), 
                    Text = post.Description.Text
                };

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