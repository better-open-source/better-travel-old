using BetterTravel.MediatR.Core;

namespace BetterTravel.Commands.Abstraction
{
    public abstract class CommandHandlerBase<TRequest, TResponse> : RequestHandlerBase<TRequest, TResponse>
        where TRequest : ICommand<TResponse>
    {
    }
}