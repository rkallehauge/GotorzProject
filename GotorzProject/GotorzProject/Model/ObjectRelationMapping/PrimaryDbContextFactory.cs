using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace GotorzProject.Model.ObjectRelationMapping
{
    public class PrimaryDbContextFactory : IDesignTimeDbContextFactory<PrimaryDbContext>
    {
        public PrimaryDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<PrimaryDbContext>();
            optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=Gotorz;Trusted_Connection=True;MultipleActiveResultSets=true");

            return new PrimaryDbContext(optionsBuilder.Options);
        }
    }
}
