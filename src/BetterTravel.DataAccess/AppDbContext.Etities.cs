using BetterTravel.DataAccess.Models;
using Microsoft.EntityFrameworkCore;

namespace BetterTravel.DataAccess
{
    public sealed partial class AppDbContext
    {
        public DbSet<TourInfo> ToursInfo { get; set; }
    }
}