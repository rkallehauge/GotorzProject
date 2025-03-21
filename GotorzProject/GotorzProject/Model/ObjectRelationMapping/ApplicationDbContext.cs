using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace GotorzProject.Model.ObjectRelationMapping
{
    public class ApplicationDbContext : IdentityDbContext
    {

        public DbSet<Customer> Customers { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<FlightDeparture> FlightDepartures { get; set; }    
        public DbSet<Hotel> Hotels { get; set; }    
        public DbSet<Order> Orders { get; set; }
        public DbSet<TravelPackage> TravelPackages { get; set; }


        public ApplicationDbContext(DbContextOptions options) : base(options) 
        {

        }
    }
}
