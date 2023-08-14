using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using TicketBooking.API.Helper;

namespace TicketBooking.API.DBContext
{
    class DbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
  {
    ApplicationDbContext IDesignTimeDbContextFactory<ApplicationDbContext>.CreateDbContext(string[] args)
    {
      var connectionString = ConfigurationHelper.Database;

      var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
      
      optionsBuilder.UseSqlServer(connectionString);

      return new ApplicationDbContext(optionsBuilder.Options);
    }
  }
}