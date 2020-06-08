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
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _dbContext;

        public UserRepository(AppDbContext dbContext) => 
            _dbContext = dbContext;

        public async Task CreateAsync(User user)
        {
            await _dbContext.Users.AddAsync(user);
            await _dbContext.SaveChangesAsync();
        }

        public async Task SubscribeAsync(long chatId) => 
            await SetSubscriptionStatus(chatId, true);

        public async Task UnsubscribeAsync(long chatId) => 
            await SetSubscriptionStatus(chatId, false);

        public async Task<User> GetFirstAsync(Expression<Func<User, bool>> wherePredicate) =>
            await _dbContext.Users.FirstOrDefaultAsync(wherePredicate);

        public async Task<List<User>> GetAllAsync(Expression<Func<User, bool>> wherePredicate) =>
            await _dbContext.Users
                .Where(wherePredicate)
                .ToListAsync();

        public async Task<List<TResult>> GetAllAsync<TResult>(
            Expression<Func<User, bool>> wherePredicate,
            Expression<Func<User, TResult>> projection) =>
            await _dbContext.Users
                .Where(wherePredicate)
                .Select(projection)
                .ToListAsync();

        private async Task SetSubscriptionStatus(long chatId, bool isSubscribed)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.ChatId == chatId);
            if (user is null) return;

            user.IsSubscribed = isSubscribed;
            await _dbContext.SaveChangesAsync();
        }
    }
}