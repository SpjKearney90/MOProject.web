using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

using Microsoft.EntityFrameworkCore;
using MOProject.Areas.Identity.Data;
using RPProject.Models;



namespace RPProject.Areas.Identity.Data

{

    public class RPProjectContext : IdentityDbContext<RPProjectUser>

    {
        public RPProjectContext(DbContextOptions<RPProjectContext> options)

            : base(options)

        {

        }

        protected override void OnModelCreating(ModelBuilder builder)

        {

            base.OnModelCreating(builder);



            // If needed, configure additional relationships here for future scalability

        }

    }

}