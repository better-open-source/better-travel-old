using System.Collections.Generic;
using System.Threading.Tasks;
using BetterTravel.Infrastructure.Domain;

namespace BetterTravel.Infrastructure.Parsers.Abstractions
{
    public interface ITagParser
    {
        Task<IEnumerable<PostInfo>> GetPostsAsync();
    }
}