using PuppeteerSharp;
using Serilog;
using ILogger = Serilog.ILogger;

namespace BetterTravel.Infrastructure.Parsers.Abstractions
{
    public abstract class BaseParser<T> where T : class
    {
        protected readonly Page Page;
        protected readonly ILogger Logger;
        
        protected BaseParser(IBrowserPageFactory pageFactory)
        {
            Logger = Log.ForContext<T>();
            Page = pageFactory.ConcretePageAsync(true)
                .ConfigureAwait(false)
                .GetAwaiter()
                .GetResult();
        }
    }
}