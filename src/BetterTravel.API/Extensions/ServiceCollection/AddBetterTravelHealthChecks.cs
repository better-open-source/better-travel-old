using BetterTravel.DataAccess.EF;
using Microsoft.Extensions.DependencyInjection;

namespace BetterTravel.API.Extensions.ServiceCollection
{
    public static partial class ServiceCollectionExtensions
    {
        public static IServiceCollection AddBetterTravelHealthChecks(this IServiceCollection services) =>
            services
                .AddHealthChecks()
                .AddDbContextCheck<AppDbContext>()
                .Services;
    }
}