
using GotorzProject.Model.Auth;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace GotorzProject.Model.ObjectRelationMapping
{
    public class ApplicationIdentityDbContext : IdentityDbContext<IdentityUser>
    {
        //public ApplicationIdentityDbContext(DbContextOptions<ApplicationIdentityDbContext> options) : base(options) { }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<FlightDeparture> FlightDepartures { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<TravelPackage> TravelPackages { get; set; }
        public DbSet<Hotel> Hotels { get; set; }
        // todo : to be deleted?
        public DbSet<CustomToken> CustomTokens { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }

        public ApplicationIdentityDbContext(DbContextOptions<ApplicationIdentityDbContext> options)
           : base(options) { }

    }
}
