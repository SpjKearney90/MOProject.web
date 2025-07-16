// using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
// using Microsoft.EntityFrameworkCore;
// using MOProject.Areas.Identity.Data;
// using MOProject.Models;

//Disbled DB 

// namespace MOProject.Data
// {
//     public class ApplicationDbContext : IdentityDbContext<RPProjectUser>
//     {
//         // Constructor to pass options to the base class
//         public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

//         // Define DbSets for your application models (Post, Page, Setting, etc.)
//         public DbSet<Post> Posts { get; set; }
//         public DbSet<Page> Pages { get; set; }
//         public DbSet<Setting> Settings { get; set; }

//         // Override OnModelCreating to configure additional model relationships if needed
//         protected override void OnModelCreating(ModelBuilder builder)
//         {
//             base.OnModelCreating(builder);

//             // Additional customizations can be made here if necessary
//             // For example:
//             // builder.Entity<Post>().HasIndex(p => p.Title).IsUnique();
//         }
//     }
// }
