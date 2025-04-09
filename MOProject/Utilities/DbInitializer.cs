using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MOProject.Data;

namespace MOProject.Utilities
{
    public class DbInitializer : IDbInitializer
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<DbInitializer> _logger;

        public DbInitializer(ApplicationDbContext context, ILogger<DbInitializer> logger)
        {
            _context = context;
            _logger = logger;
        }

        public void Initialize()
        {
            _logger.LogInformation("Applying migrations (if any).");

            try
            {
                _context.Database.Migrate(); // Applies any pending migrations
                _logger.LogInformation("Database is up to date.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while migrating the database.");
                throw;
            }
        }

        public Task InitializeAsync()
        {
            throw new NotImplementedException();
        }
    }
}
