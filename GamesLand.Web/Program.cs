using FluentMigrator.Runner;
using GamesLand.Web;
using GamesLand.Web.Middlewares;

int envPort = Convert.ToInt32(Environment.GetEnvironmentVariable("PORT"));
int port = envPort != 0 ? envPort : 5000;
var builder = WebApplication.CreateBuilder(args);
builder.WebHost.ConfigureKestrel(options => options.ListenLocalhost(port));
var configuration = builder.Configuration;
var services = builder.Services;

configuration.AddJsonFile("./config.json", true);
configuration.AddJsonFile($"./config.{builder.Environment.EnvironmentName}.json", true);
services.AddServices(builder.Configuration);

var app = builder.Build();
using var scope = app.Services.CreateScope();
// Run Migrations
var migrationRunner = scope.ServiceProvider.GetService<IMigrationRunner>();
migrationRunner?.MigrateUp();

app.UseMiddleware<ExceptionMiddleware>();
app.UseCors(policyBuilder =>
{
    policyBuilder.AllowAnyOrigin();
    policyBuilder.AllowAnyHeader();
});
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();

public partial class Program
{
}