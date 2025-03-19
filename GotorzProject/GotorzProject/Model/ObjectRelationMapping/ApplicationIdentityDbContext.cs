using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace GotorzProject.Model.ObjectRelationMapping
{
    public class ApplicationIdentityDbContext : IdentityDbContext
    {
        //public ApplicationIdentityDbContext(DbContextOptions<ApplicationIdentityDbContext> options) : base(options) { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }

        public ApplicationIdentityDbContext(DbContextOptions<ApplicationIdentityDbContext> options)
           : base(options) { }

    }
}
