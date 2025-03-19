using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.IdentityModel.Protocols.Configuration;

namespace GotorzProject.Model.ObjectRelationMapping
{
    public class ApplicationIdentityDbContextFactory : IDesignTimeDbContextFactory<ApplicationIdentityDbContext>
    {
        public ApplicationIdentityDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationIdentityDbContext>();

            var configuration = new ConfigurationBuilder()
            .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
            .AddJsonFile("appsettings.json")
            .AddJsonFile("appsettings.Development.json")
            .Build();

            string? curDb = (string?)configuration.GetValue(typeof(string), "CurrentUsedDB");

            if (string.IsNullOrEmpty(curDb))
            {
                throw new InvalidConfigurationException("No database configured.");
            }
            string? connString = configuration.GetConnectionString(curDb);

            if (string.IsNullOrEmpty(connString))
            {
                throw new InvalidConfigurationException("No database connection string configured.");
            }

            if (curDb == "MSSql")
            {
                optionsBuilder.UseSqlServer(connString);
            }
            else if (curDb == "PostgreSQL")
            {
                optionsBuilder.UseNpgsql(connString);
            }
            else
            {
                throw new InvalidConfigurationException("Invalid databse type configured.");
            }

            return new ApplicationIdentityDbContext(optionsBuilder.Options);
        }
    }
}
