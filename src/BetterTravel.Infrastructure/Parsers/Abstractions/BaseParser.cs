using System;
using PuppeteerSharp;

namespace BetterTravel.Infrastructure.Parsers.Abstractions
{
    public abstract class BaseParser
    {
        protected readonly Page Page;

        protected BaseParser(IBrowserPageFactory pageFactory) => 
            Page = pageFactory.ConcretePageAsync(true)
                .ConfigureAwait(false)
                .GetAwaiter()
                .GetResult();
    }
}