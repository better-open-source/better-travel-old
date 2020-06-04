using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;

namespace BetterTravel.API.Extensions.ServiceCollection
{
    public static partial class ServiceCollectionExtensions
    {
        public static IServiceCollection AddBetterTravelRouteOptions(this IServiceCollection services)
        {
            return services.Configure<RouteOptions>(SetupRouteOptions);

            static void SetupRouteOptions(RouteOptions options)
            {
                options.LowercaseUrls = true;
                options.LowercaseQueryStrings = true;
                options.AppendTrailingSlash = false;
            }
        }
    }
}