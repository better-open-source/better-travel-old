using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BetterTravel.Common
{
    public static class Helpers
    {
        public static async Task<IEnumerable<T>> WhenAll<T>(this IEnumerable<Task<T>> tasks) =>
            await Task.WhenAll(tasks);

        public static async Task<IEnumerable<T>> WhenAllSync<T>(this IEnumerable<Task<T>> tasks) => 
            await Task.FromResult(tasks.Select(t => t.GetAwaiter().GetResult()));

        public static TResult Log<TResult>(this TResult res, string log)
        {
            System.Console.WriteLine(log);
            return res;
        }
    }
}