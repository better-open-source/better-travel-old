using System.Diagnostics.CodeAnalysis;
using Autofac;
using BetterTravel.API.Extensions.Configuration;
using BetterTravel.API.HostedServices;
using BetterTravel.Common.Configurations;
using MediatR;
using Microsoft.Extensions.Configuration;
using Telegram.Bot;

namespace BetterTravel.API.IoC
{
    [ExcludeFromCodeCoverage]
    public class ApiModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            RegisterMediator(builder);
            RegisterTelegram(builder);
        }

        private static void RegisterTelegram(ContainerBuilder builder)
        {
            builder
                .Register<BotConfiguration>(context =>
                {
                    var c = context.Resolve<IConfiguration>();
                    return c.GetOptions<BotConfiguration>(nameof(BotConfiguration));
                }).SingleInstance();

            builder
                .Register(context =>
                {
                    var config = context.Resolve<BotConfiguration>();
                    return new TelegramBotClient(config.BotToken);
                }).As<ITelegramBotClient>()
                .SingleInstance();
        }

        private static void RegisterMediator(ContainerBuilder builder)
        {
            builder.RegisterType<Mediator>()
                .As<IMediator>()
                .InstancePerLifetimeScope();

            builder.Register<ServiceFactory>(context =>
            {
                var c = context.Resolve<IComponentContext>();
                return t => c.Resolve(t);
            });
        }
    }
}