using System;
using System.Threading.Tasks;
using Autofac.Extensions.DependencyInjection;
using BetterTravel.API.Extensions.Host;
using BetterTravel.DataAccess.EF;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace BetterTravel.API
{
    public static class Program
    {
        public static Task Main(string[] args) =>
            RunAsync(CreateHostBuilder(args).Build());

        private static async Task RunAsync(IHost host)
        {
            Log.Logger = BuildLogger(host);

            try
            {
                Log.Information("Downloading browser...");
                await host.DownloadBrowserAsync();
                
                Log.Information("Ensure database created...");
                await host.MigrateDatabaseAsync<AppDbContext>();
                
                Log.Information("Starting web host...");
                await host.RunAsync();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Host terminated unexpectedly");
                throw;
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        private static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseSerilog()
                .UseServiceProviderFactory(new AutofacServiceProviderFactory())
                .ConfigureWebHostDefaults(hostBuilder => hostBuilder
                    .UseShutdownTimeout(TimeSpan.FromMinutes(5))
                    .UseStartup<Startup>());

        private static ILogger BuildLogger(IHost host) =>
            new LoggerConfiguration()
                .ReadFrom.Configuration(host.Services.GetRequiredService<IConfiguration>())
                .CreateLogger();
    }
}