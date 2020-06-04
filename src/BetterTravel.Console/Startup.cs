using System;
using BetterTravel.Common;
using BetterTravel.DataAccess.Abstractions.Repositories;
using BetterTravel.DataAccess.EF;
using BetterTravel.DataAccess.EF.Repositories;
using BetterTravel.Infrastructure;
using BetterTravel.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace BetterTravel.Console
{
    internal sealed class Startup
    {
        private const string ConnectionString = 
            "Server=localhost,1433;Database=BetterTravelDb;User Id=SA;Password=MyStr0ngP@ssWorD;";

        public ServiceProvider ConfigureServices()
        {
            var services = new ServiceCollection();

            services
                .AddDbContext<AppDbContext>(builder => builder.UseSqlServer(ConnectionString))
                .AddTransient<ITourInfoRepository, TourInfoRepository>()
                .AddTransient<IAuthService, AuthService>()
                .AddScoped<IBrowserPageFactory, BrowserPageFactory>(ResolveBrowserPageFactory)
                .AddScoped<ITestService, TestService>()
                .AddLogging(builder => builder.AddConsole());
            
            return services.BuildServiceProvider();
        }

        private static BrowserPageFactory ResolveBrowserPageFactory(IServiceProvider sp)
        {
            var logger = sp.GetService<ILoggerFactory>().CreateLogger<BrowserPageFactory>();
            var authService = sp.GetService<IAuthService>();
            var cookies = authService.AuthenticateAsync(
                    InstagramConfiguration.Username, 
                    InstagramConfiguration.Password, 
                    2500)
                .ConfigureAwait(false)
                .GetAwaiter()
                .GetResult();
            
            return new BrowserPageFactory(cookies);
        }
    }
}