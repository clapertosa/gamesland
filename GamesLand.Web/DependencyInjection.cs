using FluentValidation.AspNetCore;
using GamesLand.Infrastructure.PostgreSQL;
using GamesLand.Web.Users.Validators;

namespace GamesLand.Web;

public static class DependencyInjection
{
    public static void AddServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddFluentValidation(fv => fv.RegisterValidatorsFromAssembly(typeof(CreateUserValidator).Assembly));
        services.AddControllers();
        services.AddPostgreSqlInfrastructure(configuration);
    }
}