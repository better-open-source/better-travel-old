using System.Collections.Generic;
using System.Threading.Tasks;
using BetterTravel.Console.Domain;

namespace BetterTravel.Console.Parsers.Abstractions
{
    public interface ITagParser
    {
        Task<IEnumerable<PostInfo>> GetPosts();
    }
}