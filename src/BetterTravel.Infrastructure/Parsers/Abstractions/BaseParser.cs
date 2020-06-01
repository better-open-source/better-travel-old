using Microsoft.Extensions.Logging;
using PuppeteerSharp;

namespace BetterTravel.Infrastructure.Parsers.Abstractions
{
    public abstract class BaseParser<T> where T : class
    {
        protected readonly Page Page;
        protected readonly ILogger<T> Logger;
        
        protected BaseParser(IBrowserPageFactory pageFactory, ILogger<T> logger)
        {
            Logger = logger;
            Page = pageFactory.ConcretePageAsync(true)
                .ConfigureAwait(false)
                .GetAwaiter()
                .GetResult();
        }
    }
}