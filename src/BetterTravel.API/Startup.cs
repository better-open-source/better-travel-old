using Autofac;
using AutoMapper;
using BetterTravel.API.Extensions.ApplicationBuilder;
using BetterTravel.API.Extensions.ServiceCollection;
using BetterTravel.API.HostedServices;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

[assembly: ApiController]
[assembly: ApiConventionType(typeof(DefaultApiConventions))]
namespace BetterTravel.API
{
    public class Startup
    {
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration) => 
            _configuration = configuration;
        
        public void ConfigureServices(IServiceCollection services) =>
            services
                .AddOptions()
                .AddAutoMapper(typeof(Startup).Assembly)
                .AddBetterTravelMvc()
                .AddBetterTravelProfiler()
                .AddBetterTravelDb(_configuration)
                .AddMemoryCache()
                .AddBetterTravelCors()
                .AddBetterTravelRouteOptions()
                .AddBetterTravelHealthChecks()
                .AddBetterTravelSwagger()
                .AddHostedService<ToursNotifierHostedService>()
                .AddHostedService<TelegramHostedService>();

        public static void ConfigureContainer(ContainerBuilder builder) =>
            builder.RegisterAssemblyModules(typeof(Startup).Assembly);

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env) =>
            app
                .UseHttpsRedirection()
                .UseStaticFiles()
                .UseBetterTravelSwaggerUi()
                .UseSerilogRequestLogging()
                .UseCors()
                .UseAuthentication()
                .UseRouting()
                .UseEndpoints(e =>
                {
                    e.MapControllers();
                    e.MapHealthChecks("/_health");
                });
    }
}