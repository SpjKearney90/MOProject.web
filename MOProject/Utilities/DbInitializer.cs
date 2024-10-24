using Microsoft.AspNetCore.Identity;
using MOProject.Data;
using MOProject.Models;
using Microsoft.Extensions.Logging;

namespace MOProject.Utilities
{
    public class DbInitializer : IDbInitializer
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ILogger<DbInitializer> _logger;

        public DbInitializer(ApplicationDbContext context,
                             UserManager<ApplicationUser> userManager,
                             RoleManager<IdentityRole> roleManager,
                             ILogger<DbInitializer> logger)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
            _logger = logger;
        }

        public void Initialize()
        {
            _logger.LogInformation("Starting initialization.");

            if (!_roleManager.RoleExistsAsync(WebsiteRoles.WebsiteAdmin!).GetAwaiter().GetResult())
            {
                _roleManager.CreateAsync(new IdentityRole(WebsiteRoles.WebsiteAdmin!)).GetAwaiter().GetResult();
                _logger.LogInformation("Created WebsiteAdmin role.");
            }

            if (!_roleManager.RoleExistsAsync(WebsiteRoles.WebsiteAuthor!).GetAwaiter().GetResult())
            {
                _roleManager.CreateAsync(new IdentityRole(WebsiteRoles.WebsiteAuthor!)).GetAwaiter().GetResult();
                _logger.LogInformation("Created WebsiteAuthor role.");
            }

            var userExists = _userManager.FindByEmailAsync("admin@gmail.com").GetAwaiter().GetResult();
            if (userExists == null)
            {
                _logger.LogInformation("Creating admin user.");
                var user = new ApplicationUser()
                {
                    UserName = "admin@gmail.com",
                    Email = "admin@gmail.com",
                    FirstName = "Super",
                    LastName = "Admin",
                    EmailConfirmed = true,  // Ensure email is confirmed
                    NormalizedEmail = "ADMIN@GMAIL.COM",
                    NormalizedUserName = "ADMIN@GMAIL.COM",
                    PhoneNumberConfirmed = false,
                    TwoFactorEnabled = false,
                    LockoutEnabled = true,
                    AccessFailedCount = 0
                };
                var result = _userManager.CreateAsync(user, "Admin1234!").GetAwaiter().GetResult();

                if (result.Succeeded)
                {
                    _logger.LogInformation("Admin user created successfully.");
                    _userManager.AddToRoleAsync(user, WebsiteRoles.WebsiteAdmin!).GetAwaiter().GetResult();
                    _logger.LogInformation("Admin user assigned to WebsiteAdmin role.");
                }
                else
                {
                    _logger.LogError("Error creating admin user: {Errors}", string.Join(", ", result.Errors.Select(e => e.Description)));
                }
            }

            var listOfPages = new List<Page>()
            {
                new Page() { Title = "About Us", Slug = "about" },
                new Page() { Title = "Contact Us", Slug = "contact" },
                new Page() { Title = "Privacy Policy", Slug = "privacy" }
            };

            _context.Pages!.AddRange(listOfPages);
            _context.SaveChanges();
            _logger.LogInformation("Pages added successfully.");
        }
    }
}
