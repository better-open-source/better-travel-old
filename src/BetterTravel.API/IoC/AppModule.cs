using System.Diagnostics.CodeAnalysis;
using Autofac;
using BetterTravel.Common;
using BetterTravel.Common.Configurations;
using BetterTravel.Infrastructure;
using BetterTravel.Services;

namespace BetterTravel.API.IoC
{
    [ExcludeFromCodeCoverage]
    public class AppModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            RegisterServices(builder);
            ResolveBrowserPageFactory(builder);
        }

        private static void RegisterServices(ContainerBuilder builder)
        {
            builder
                .RegisterType<AuthService>()
                .As<IAuthService>();
            
            builder
                .RegisterType<ToursFetcherService>()
                .As<IToursFetcherService>();
        }

        private static void ResolveBrowserPageFactory(ContainerBuilder builder)
        {
            builder.Register(ctx =>
                {
                    var authService = ctx.Resolve<IAuthService>();
                    var cookies = authService.AuthenticateAsync(
                            InstagramConfiguration.Username,
                            InstagramConfiguration.Password,
                            2500)
                        .ConfigureAwait(false)
                        .GetAwaiter()
                        .GetResult();

                    return new BrowserPageFactory(cookies);
                }).As<IBrowserPageFactory>()
                .InstancePerDependency();
        }
    }
}