using BetterTravel.MediatR.Core.HandlerResults.Abstractions;
using MediatR;

namespace BetterTravel.Commands.Abstraction
{
    public interface ICommand<T> : IRequest<IHandlerResult<T>>
    {
    }
}