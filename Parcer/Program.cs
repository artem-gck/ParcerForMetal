using Microsoft.EntityFrameworkCore;
using Parser.DataAccess;
using Parser.DataAccess.SqlServer;
using Parser.Services.Logic;
using Parser.Serviñes;
using Parser.Serviñes.Models.Context;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddDbContext<MetalContext>(opt =>
    opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
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