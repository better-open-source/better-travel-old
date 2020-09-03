using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using Autofac;
using BetterTravel.DataAccess.Abstractions.Repositories;
using BetterTravel.DataAccess.EF;
using BetterTravel.DataAccess.EF.Repositories;

namespace BetterTravel.API.IoC
{
    [ExcludeFromCodeCoverage]
    public class DataAccessModule : Autofac.Module
    {
        protected override Assembly ThisAssembly => typeof(AppDbContext).Assembly;

        protected override void Load(ContainerBuilder builder)
        {
            builder
                .RegisterType<TourRepository>()
                .As<ITourRepository>();
            
            builder
                .RegisterType<UserRepository>()
                .As<IUserRepository>();
        }
    }
}