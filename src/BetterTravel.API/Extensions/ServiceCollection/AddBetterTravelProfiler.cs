using Microsoft.Extensions.DependencyInjection;

namespace BetterTravel.API.Extensions.ServiceCollection
{
    public static partial class ServiceCollectionExtensions
    {
        public static IServiceCollection AddBetterTravelProfiler(this IServiceCollection services) =>
            services
                .AddMiniProfiler()
                .AddEntityFramework()
                .Services;
    }
}