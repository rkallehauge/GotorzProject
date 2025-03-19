using GotorzProject.Model.Auth;
using Microsoft.EntityFrameworkCore;

namespace GotorzProject.Model.ObjectRelationMapping
{
    public class PrimaryDbContext : DbContext
    {
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<FlightDeparture> FlightDepartures { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<TravelPackage> TravelPackages { get; set; }
        public DbSet<Hotel> Hotels { get; set; }

        public DbSet<CustomToken> CustomTokens { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }

        public PrimaryDbContext(DbContextOptions<PrimaryDbContext> options)
           : base(options) { }


    }
}
