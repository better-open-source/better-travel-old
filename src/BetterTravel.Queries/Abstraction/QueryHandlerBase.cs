using BetterTravel.MediatR.Core;

namespace BetterTravel.Queries.Abstraction
{
    public abstract class QueryHandlerBase<TRequest, TResponse> : RequestHandlerBase<TRequest, TResponse>
        where TRequest : IQuery<TResponse>
    {
    }
}