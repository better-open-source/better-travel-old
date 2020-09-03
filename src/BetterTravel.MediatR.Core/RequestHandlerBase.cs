using System.Threading;
using System.Threading.Tasks;
using BetterTravel.MediatR.Core.HandlerResults;
using BetterTravel.MediatR.Core.HandlerResults.Abstractions;
using MediatR;

namespace BetterTravel.MediatR.Core
{
    public abstract class RequestHandlerBase<TRequest, TResponse> : IRequestHandler<TRequest, IHandlerResult<TResponse>>
        where TRequest : IRequest<IHandlerResult<TResponse>>
    {
        public abstract Task<IHandlerResult<TResponse>> Handle(TRequest request, CancellationToken cancellationToken);

        protected static IHandlerResult<TResponse> Ok() =>
            new OkHandlerResult<TResponse>();

        protected static IHandlerResult<TResponse> Data(TResponse data) =>
            new DataHandlerResult<TResponse>(data);

        protected static IHandlerResult<TResponse> ValidationFailed(string message) =>
            new ValidationFailedHandlerResult<TResponse>(message);
    }
}