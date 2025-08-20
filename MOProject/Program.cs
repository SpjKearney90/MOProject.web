using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

// using MOProject.Data;
using MOProject.Models;
// using MOProject.Services;
// using MOProject.Utilities;
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
builder.Services.AddTransient<IEmailService, EmailService>();


// === Database and Identity setup (DISABLED) ===

// builder.Services.AddDbContext<ApplicationDbContext>(options =>
//     options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// builder.Services.AddDefaultIdentity<RPProjectUser>(options => options.SignIn.RequireConfirmedAccount = true)
//     .AddEntityFrameworkStores<ApplicationDbContext>();

// === Application services (non-database dependent) ===

builder.Services.AddRazorPages();
builder.Services.AddControllersWithViews();

var app = builder.Build();

// === OPTIONAL: Comment out or rewrite this if it depends on the DB ===
// SeedData(app);

// Middleware configuration
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

// These are safe to keep if you're not using Identity
// But you can comment them out if you're also disabling auth:
// app.UseAuthentication();
// app.UseAuthorization();

// Routing
app.MapRazorPages();



app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

// === Optional: Remove or refactor if this seeds database ===
// void SeedData(WebApplication app) { ... }
