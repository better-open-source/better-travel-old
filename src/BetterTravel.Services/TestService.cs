﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BetterTravel.Common;
using BetterTravel.DataAccess.Abstractions.Entities;
using BetterTravel.DataAccess.Abstractions.Repositories;
using BetterTravel.DataAccess.EF;
using BetterTravel.Domain;
using BetterTravel.Infrastructure;
using BetterTravel.Infrastructure.Parsers;
using BetterTravel.Infrastructure.Parsers.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BetterTravel.Services
{
    public interface ITestService
    {
        Task RunTestAsync();
    }

    public class TestService : ITestService
    {
        private readonly ITourInfoRepository _tourInfoRepository;
        private readonly ILoggerFactory _loggerFactory;
        private readonly AppDbContext _dbContext;
        private readonly IBrowserPageFactory _pageFactory;

        public TestService(
            AppDbContext dbContext,
            IBrowserPageFactory pageFactory,
            ITourInfoRepository tourInfoRepository, 
            ILoggerFactory loggerFactory)
        {
            _dbContext = dbContext;
            _pageFactory = pageFactory;
            _tourInfoRepository = tourInfoRepository;
            _loggerFactory = loggerFactory;
        } 

        public async Task RunTestAsync()
        {
            var testRepo = await _tourInfoRepository.GetAllAsync(t => t != null);
            var results = await GetAllPosts(Consts.HashTags);
            var tours = results
                .Where(t => !(t is null))
                .Select(MapTourInfo);

            var t1 = await _dbContext.ToursInfo.ToListAsync();
            await _dbContext.ToursInfo.AddRangeAsync(tours);
            await _dbContext.SaveChangesAsync();
            
            var t2 = await _dbContext.ToursInfo.ToListAsync();
        }
        
        private async Task<IEnumerable<PostInfo>> GetAllPosts(IEnumerable<string> tags)
        {
            var parsers = await tags.Select(InitParser).WhenAll();
            var parsersPosts = await parsers.Select(parser => parser.GetPostsAsync()).WhenAll();
            return parsersPosts.SelectMany(seq => seq);
        }

        private async Task<ITagParser> InitParser(string tag) =>
            await Task.Factory.StartNew(() =>
                new InstaTagParser(tag, _pageFactory, _loggerFactory.CreateLogger<InstaTagParser>()));

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
    }
}