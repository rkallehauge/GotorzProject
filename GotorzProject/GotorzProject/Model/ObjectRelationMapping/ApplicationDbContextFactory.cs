using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace GotorzProject.Model.ObjectRelationMapping
{
    public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
    {
        public ApplicationDbContext CreateDbContext(string[] args)
        {
            // Build config from appsettings.json
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .AddJsonFile("appsettings.Development.json")
                .AddEnvironmentVariables()
                .Build();

            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();

            string dbType = (string) configuration.GetValue(typeof(string), "CurrentUsedDB");

            var connectionString = configuration.GetConnectionString(dbType);

            if(dbType == "MSSql")
            {
                optionsBuilder.UseSqlServer(connectionString); // Or UseNpgsql, etc.
            }
            else if (dbType == "PostgreSQL")
            {
                optionsBuilder.UseNpgsql(connectionString); // Or UseNpgsql, etc.
            }

            return new ApplicationDbContext(optionsBuilder.Options);
        }
    }
}
