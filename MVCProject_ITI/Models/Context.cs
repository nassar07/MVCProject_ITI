using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace MVCProject_ITI.Models
{
    public class Context : IdentityDbContext<ApplicationUser>
    {
        DbSet<Category> Categories { get; set; }
        DbSet<Tasks> Tasks { get; set; }




        public Context(DbContextOptions<Context> options) : base(options)
        {

        }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }

    }
}
