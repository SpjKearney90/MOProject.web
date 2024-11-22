using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MOProject.Models;

namespace MOProject.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
        public DbSet<ApplicationUser>? ApplicationUsers { get; set; }
        public DbSet<Post>? Posts { get; set; }
        public DbSet<Page>? Pages { get; set; }
        public DbSet<Setting>? Settings { get; set; }
    }
}
