using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace GotorzProject.Model.ObjectRelationMapping
{
    public class ApplicationDbContext : IdentityDbContext<CustomUser>
    {

        public DbSet<FlightDeparture> FlightDepartures { get; set; }    
        public DbSet<Hotel> Hotels { get; set; }    
        public DbSet<Order> Orders { get; set; }
        public DbSet<TravelPackage> TravelPackages { get; set; }

        public DbSet<LoggedEvent> LoggedEvents { get; set; }

        public ApplicationDbContext(DbContextOptions options) : base(options) 
        {

        }
    }
}
