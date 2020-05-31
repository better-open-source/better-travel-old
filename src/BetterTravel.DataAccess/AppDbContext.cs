using Microsoft.EntityFrameworkCore;

namespace BetterTravel.DataAccess
{
    public sealed partial class AppDbContext : DbContext
    {
        public AppDbContext()
        {
            Database.EnsureCreated();
        }
 
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(
                "Server=localhost,1433;Database=ParsingData;User Id=SA;Password=MyP@ssw0rd;");
        }
    }
}