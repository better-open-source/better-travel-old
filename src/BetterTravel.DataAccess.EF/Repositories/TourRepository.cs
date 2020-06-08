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
    public class TourRepository : ITourRepository
    {
        private readonly AppDbContext _dbContext;

        public TourRepository(AppDbContext dbContext) => 
            _dbContext = dbContext;

        public async Task<List<Tour>> GetLatestAsync(int count) => 
            await _dbContext.Tours
                .OrderByDescending(t => t.StoredAt)
                .Take(count)
                .ToListAsync();

        public async Task<List<Tour>> GetLatestAsync(int count, Expression<Func<Tour, bool>> wherePredicate) =>
            await _dbContext.Tours
                .Where(wherePredicate)
                .OrderByDescending(t => t.StoredAt)
                .Take(count)
                .ToListAsync();

        public async Task<List<TResult>> GetAllAsync<TResult>(
            Expression<Func<Tour, bool>> wherePredicate,
            Expression<Func<Tour, TResult>> projection) =>
            await _dbContext.Tours
                .Where(wherePredicate)
                .Select(projection)
                .ToListAsync();

        public async Task<List<Tour>> GetAllAsync(Expression<Func<Tour, bool>> wherePredicate) =>
            await _dbContext.Tours
                .Where(wherePredicate)
                .ToListAsync();

        public async Task InsertRangeAsync(IEnumerable<Tour> tours)
        {
            await _dbContext.Tours.AddRangeAsync(tours);
            await _dbContext.SaveChangesAsync();
        }
    }
}