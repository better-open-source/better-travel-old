using Microsoft.EntityFrameworkCore;

namespace BetterTravel.DataAccess
{
    public class TourInfoContext : DbContext
    {
        public DbSet<TourInfo> ToursInfo { get; set; }
         
        public TourInfoContext()
        {
            Database.EnsureCreated();
        }
 
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=ParsingData;Trusted_Connection=True;");
        }
    }
}