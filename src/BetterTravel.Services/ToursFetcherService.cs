using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BetterTravel.Common;
using BetterTravel.DataAccess.Abstractions.Entities;
using BetterTravel.DataAccess.Abstractions.Repositories;
using BetterTravel.Infrastructure;
using BetterTravel.Infrastructure.Domain;
using BetterTravel.Infrastructure.Parsers;
using BetterTravel.Infrastructure.Parsers.Abstractions;

namespace BetterTravel.Services
{
    public interface IToursFetcherService
    {
        Task<List<Tour>> FetchToursAsync(bool cached, int count);
    }

    public class ToursFetcherService : IToursFetcherService
    {
        private readonly ITourRepository _tourRepository;
        private readonly IBrowserPageFactory _pageFactory;

        public ToursFetcherService(
            IBrowserPageFactory pageFactory,
            ITourRepository tourRepository)
        {
            _pageFactory = pageFactory;
            _tourRepository = tourRepository;
        }

        public async Task<List<Tour>> FetchToursAsync(bool cached, int count)
        {
            if (cached)
                return await _tourRepository.GetLatestAsync(count);

            var tags = new[] {"гарячітури", "тур", "поїхализнами_львів", "поїхализнами_зелена37"};
            var posts = await GetAllPosts(tags);

            var upcomingTours = posts
                .Where(p => p.PostUrl != null)
                .DistinctBy(p => p.PostUrl)
                .Select(MapTourInfo)
                .ToList();

            var cachedTours = await _tourRepository.GetLatestAsync(100);
            var cachedUrls = cachedTours.Select(tour => tour.PostUrl);
            var newTours = upcomingTours.Where(tour => cachedUrls.All(url => tour.PostUrl != url)).ToList();

            await _tourRepository.InsertRangeAsync(newTours);

            return newTours;
        }

        private async Task<IEnumerable<PostInfo>> GetAllPosts(IEnumerable<string> tags)
        {
            var parsers = await tags.Select(InitParser).WhenAll();
            var parsersPosts = await parsers.Select(parser => parser.GetPostsAsync()).WhenAll();
            return parsersPosts.SelectMany(seq => seq);
        }

        private async Task<ITagParser> InitParser(string tag) =>
            await Task.Factory.StartNew(() =>
                new InstaTagParser(tag, _pageFactory));

        private static Tour MapTourInfo(PostInfo post) =>
            new Tour
            {
                PostUrl = post?.PostUrl,
                Author = post?.Author,
                ImgUrl = post?.ImgUrl,
                Date = post?.Description?.Date,
                HashTags = string.Join(", ", post?.Description?.HashTags??new List<string>()),
                Text = post?.Description?.Text,
                StoredAt = DateTime.Now
            };
    }
}