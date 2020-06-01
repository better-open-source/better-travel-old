using BetterTravel.DataAccess.Abstractions.Entities;
using Microsoft.EntityFrameworkCore;

namespace BetterTravel.DataAccess.EF
{
    public sealed partial class AppDbContext
    {
        public DbSet<TourInfo> ToursInfo { get; set; }
    }
}