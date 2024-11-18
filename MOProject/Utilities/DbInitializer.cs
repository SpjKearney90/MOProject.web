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
        private readonly ILogger<DbInitializer> _logger;

        public DbInitializer(ApplicationDbContext context,
                             UserManager<ApplicationUser> userManager,
                             ILogger<DbInitializer> logger)
        {
            _context = context;
            _userManager = userManager;
            _logger = logger;
        }

        public void Initialize()
        {
            _logger.LogInformation("Starting initialization.");

            // Check if the admin user exists
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
                }
                else
                {
                    _logger.LogError("Error creating admin user: {Errors}", string.Join(", ", result.Errors.Select(e => e.Description)));
                }
            }
            else
            {
                _logger.LogInformation("Admin user already exists.");
            }
        }

        public Task InitializeAsync()
        {
            throw new NotImplementedException();
        }
    }
}
