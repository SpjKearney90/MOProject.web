
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MOProject.Data;
using MOProject.Models;
using MOProject.Services;
using MOProject.Utilites;
using MOProject.Utilities;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();
builder.Services.AddTransient<IEmailService, DevTimeEmailService>();




var connectingString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connectingString));

builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();


builder.Services.AddScoped<IDbInitializer, DbInitializer>();



var app = builder.Build();
DataSeeding();

if (builder.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}



// Allows us to serve index.html as the default webpage
app.UseDefaultFiles();

// Allows us to serve files from wwwroot
app.UseStaticFiles();



app.MapRazorPages();


app.MapControllerRoute(
    name: "area",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");


app.MapControllerRoute(
    name: "Default",
    pattern: "{controller=Home}/{action=Index}/{id?}");



app.Run();

void DataSeeding()


{
    using (var scope = app.Services.CreateScope())
    {
        var DbInitialize = scope.ServiceProvider.GetRequiredService<IDbInitializer>();
        DbInitialize.Initialize();
    }
}