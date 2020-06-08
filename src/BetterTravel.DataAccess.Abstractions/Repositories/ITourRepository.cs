using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using BetterTravel.DataAccess.Abstractions.Entities;

namespace BetterTravel.DataAccess.Abstractions.Repositories
{
    public interface ITourRepository
    {
        Task<List<Tour>> GetLatestAsync(int count);
        
        Task<List<Tour>> GetLatestAsync(int count, Expression<Func<Tour, bool>> wherePredicate);
        
        Task<List<Tour>> GetAllAsync(Expression<Func<Tour, bool>> wherePredicate);
        
        Task<List<TResult>> GetAllAsync<TResult>(
            Expression<Func<Tour, bool>> wherePredicate,
            Expression<Func<Tour, TResult>> projection);

        Task InsertRangeAsync(IEnumerable<Tour> tours);
    }
}