
using MOProject.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();
builder.Services.AddTransient<IEmailService, DevTimeEmailService>();

var app = builder.Build();

if (builder.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}



// Allows us to serve index.html as the default webpage
app.UseDefaultFiles();

// Allows us to serve files from wwwroot
app.UseStaticFiles();



app.MapRazorPages();


//app.Run(async ctx =>
//{
//  await ctx.Response.WriteAsync("<html><body><h1>Welcome to FreeBilling</h1></body></html>");
//});

app.Run();
