using FluentMigrator.Runner;
using GamesLand.Web;
using GamesLand.Web.Middlewares;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;
var services = builder.Services;

configuration.AddJsonFile("./config.json", false);
services.AddServices(builder.Configuration);

var app = builder.Build();
using var scope = app.Services.CreateScope();
// Run Migrations
var migrationRunner = scope.ServiceProvider.GetService<IMigrationRunner>();
migrationRunner.MigrateUp();

app.UseMiddleware<ExceptionMiddleware>();
app.MapControllers();

app.Run();