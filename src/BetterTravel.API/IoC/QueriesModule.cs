using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using Autofac;
using BetterTravel.Queries.Abstraction;
using MediatR;

namespace BetterTravel.API.IoC
{
    [ExcludeFromCodeCoverage]
    public class QueriesModule : Autofac.Module
    {
        protected override Assembly ThisAssembly => typeof(QueryHandlerBase<,>).Assembly;

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(ThisAssembly)
                .AsClosedTypesOf(typeof(IRequestHandler<,>))
                .AsImplementedInterfaces();
        }
    }
}