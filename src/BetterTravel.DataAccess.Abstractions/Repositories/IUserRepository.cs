using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using BetterTravel.DataAccess.Abstractions.Entities;

namespace BetterTravel.DataAccess.Abstractions.Repositories
{
    public interface IUserRepository
    {
        Task CreateAsync(User user);
        
        Task SubscribeAsync(long chatId);
        
        Task UnsubscribeAsync(long chatId);
        
        Task<User> GetFirstAsync(Expression<Func<User, bool>> wherePredicate);
        
        Task<List<User>> GetAllAsync(Expression<Func<User, bool>> wherePredicate);
        
        Task<List<TResult>> GetAllAsync<TResult>(
            Expression<Func<User, bool>> wherePredicate,
            Expression<Func<User, TResult>> projection);
    }
}