using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MOProject.Areas.Identity.Data;
using MOProject.Data;
using MOProject.Models;
using MOProject.Services;
using MOProject.Utilities;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Configure Serilog for logging
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .WriteTo.Console()
    .WriteTo.File("logs/myapp-.log", rollingInterval: RollingInterval.Day)
    .CreateLogger();

builder.Host.UseSerilog();
builder.Logging.ClearProviders();
builder.Logging.AddSerilog();

// Register ApplicationDbContext with RPProjectUser
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Register Identity services with RPProjectUser and ApplicationDbContext
builder.Services.AddDefaultIdentity<RPProjectUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>(); // Link with ApplicationDbContext

// Register additional services
builder.Services.AddScoped<IDbInitializer, DbInitializer>(); // Replace with your actual implementation
builder.Services.AddScoped<UserManager<RPProjectUser>>(); // Correct UserManager registration
builder.Services.AddScoped<SignInManager<RPProjectUser>>(); // Correct SignInManager registration
builder.Services.AddTransient<IEmailService, DevTimeEmailService>();

builder.Services.AddRazorPages();
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Seed initial data
SeedData(app);

// Configure middleware
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

// Define routing
app.MapRazorPages();

app.MapControllerRoute(
    name: "area",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

// Data seeding method
void SeedData(WebApplication app)
{
    using (var scope = app.Services.CreateScope())
    {
        var dbInitializer = scope.ServiceProvider.GetRequiredService<IDbInitializer>();
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();

        try
        {
            dbInitializer.Initialize();
            logger.LogInformation("Data seeding completed successfully.");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error during data seeding.");
            throw;
        }
    }
}
