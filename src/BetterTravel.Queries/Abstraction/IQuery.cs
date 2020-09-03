using BetterTravel.MediatR.Core.HandlerResults.Abstractions;
using MediatR;

namespace BetterTravel.Queries.Abstraction
{
    public interface IQuery<T> : IRequest<IHandlerResult<T>>
    {
    }
}