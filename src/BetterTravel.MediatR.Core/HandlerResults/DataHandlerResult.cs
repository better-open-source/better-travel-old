using BetterTravel.MediatR.Core.HandlerResults.Abstractions;

namespace BetterTravel.MediatR.Core.HandlerResults
{
    public class DataHandlerResult<T> : IHandlerResult<T>
    {
        public DataHandlerResult(T data) => 
            Data = data;

        public T Data { get; }
    }
}