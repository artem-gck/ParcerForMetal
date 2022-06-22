using Microsoft.EntityFrameworkCore;
using Parser.DataAccess;
using Parser.DataAccess.SqlServer;
using Parser.Services.Logic;
using Parser.Serviñes;
using Parser.Serviñes.Models.Context;

var builder = WebApplication.CreateBuilder(args);
Host.CreateDefaultBuilder(args)
.ConfigureWebHostDefaults(webBuilder =>
{

    var port = Environment.GetEnvironmentVariable("PORT");

    webBuilder.UseUrls($"http://+:{port}");

    //webBuilder.UseStartup<Startup>();

});
// Add services to the container.

var defaultConnectionString = string.Empty;

if (builder.Environment.EnvironmentName == "Development")
{
    defaultConnectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    //var connectionUrl = /*Environment.GetEnvironmentVariable("DATABASE_URL");*/ "postgres://pcgeuxxctxtdrg:4389638e9b54fbea09186de13cc5b2563f9120bcfd3afe245d4cacf2cdcf9a75@ec2-52-212-228-71.eu-west-1.compute.amazonaws.com:5432/d7bm7ameohjgqa";

    //connectionUrl = connectionUrl.Replace("postgres://", string.Empty);
    //var userPassSide = connectionUrl.Split("@")[0];
    //var hostSide = connectionUrl.Split("@")[1];

    //var user = userPassSide.Split(":")[0];
    //var password = userPassSide.Split(":")[1];
    //var host = hostSide.Split("/")[0];
    //var database = hostSide.Split("/")[1].Split("?")[0];

    //defaultConnectionString = $"Host={host};Database={database};Username={user};Password={password};SSL Mode=Require;Trust Server Certificate=true";
}
else
{
    // Use connection string provided at runtime by Heroku.
    var connectionUrl = /*Environment.GetEnvironmentVariable("DATABASE_URL");*/ Environment.GetEnvironmentVariable("DATABASE_URL");

    connectionUrl = connectionUrl.Replace("postgres://", string.Empty);
    var userPassSide = connectionUrl.Split("@")[0];
    var hostSide = connectionUrl.Split("@")[1];

    var user = userPassSide.Split(":")[0];
    var password = userPassSide.Split(":")[1];
    var host = hostSide.Split("/")[0];
    var database = hostSide.Split("/")[1].Split("?")[0];

    defaultConnectionString = $"Host={host};Database={database};Username={user};Password={password};SSL Mode=Require;Trust Server Certificate=true";
}

builder.Services.AddDbContext<MetalContext>(opt =>
    opt.UseNpgsql(defaultConnectionString));

builder.Services.AddControllersWithViews();
builder.Services.AddTransient<IMetalManager, MetalManager>();
builder.Services.AddTransient<ITokenManager, TokenManager>();
builder.Services.AddTransient<IAuthManager, AuthManager>();
builder.Services.AddTransient<IMetalAccessManager, MetalAccessSqlServerManager>();
builder.Services.AddTransient<IAuthAccessManager, AuthAccessManager>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler();
}

//app.UseHttpsRedirection();
app.UseStatusCodePages();
app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();