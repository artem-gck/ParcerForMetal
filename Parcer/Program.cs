using Microsoft.EntityFrameworkCore;
using Parser.DataAccess;
using Parser.DataAccess.SqlServer;
using Parser.Services.Logic;
using Parser.Serviñes;
using Parser.Serviñes.Models.Context;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddDbContext<MetalContext>(opt =>
    opt.UseSqlServer(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=MetalManagment;Integrated Security=True;"));
builder.Services.AddControllersWithViews();
builder.Services.AddTransient<IParserManager, ParserManager>();
builder.Services.AddTransient<IDataAccessManager, DataAccessSqlServerManager>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler();
}

app.UseHttpsRedirection();
app.UseStatusCodePages();
app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();