using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using BetterTravel.DataAccess.Abstractions.Entities;
using BetterTravel.DataAccess.Abstractions.Repositories;
using Microsoft.EntityFrameworkCore;

namespace BetterTravel.DataAccess.EF.Repositories
{
    public class TourInfoRepository : ITourInfoRepository
    {
        private readonly AppDbContext _dbContext;

        public TourInfoRepository(AppDbContext dbContext) => 
            _dbContext = dbContext;

        public async Task<List<TResult>> GetAllAsync<TResult>(
            Expression<Func<TourInfo, bool>> wherePredicate,
            Expression<Func<TourInfo, TResult>> projection) =>
            await _dbContext.ToursInfo
                .Where(wherePredicate)
                .Select(projection)
                .ToListAsync();

        public async Task<List<TourInfo>> GetAllAsync(Expression<Func<TourInfo, bool>> wherePredicate) =>
            await _dbContext.ToursInfo
                .Where(wherePredicate)
                .ToListAsync();
    }
}