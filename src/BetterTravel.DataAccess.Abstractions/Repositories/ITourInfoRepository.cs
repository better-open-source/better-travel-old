using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using BetterTravel.DataAccess.Abstractions.Entities;

namespace BetterTravel.DataAccess.Abstractions.Repositories
{
    public interface ITourInfoRepository
    {
        Task<List<TourInfo>> GetAllAsync(Expression<Func<TourInfo, bool>> wherePredicate);
        Task<List<TResult>> GetAllAsync<TResult>(
            Expression<Func<TourInfo, bool>> wherePredicate,
            Expression<Func<TourInfo, TResult>> projection);
    }
}