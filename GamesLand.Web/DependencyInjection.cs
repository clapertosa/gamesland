using System.Text;
using FluentValidation.AspNetCore;
using GamesLand.Infrastructure.PostgreSQL;
using GamesLand.Infrastructure.RAWG.Services;
using GamesLand.Web.Users.Validators;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace GamesLand.Web;

public static class DependencyInjection
{
    public static void AddServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddFluentValidation(fv => fv.RegisterValidatorsFromAssembly(typeof(CreateUserValidator).Assembly));
        services.AddControllers();
        services.AddPostgreSqlInfrastructure(configuration);

        // Authentication
        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(options =>
        {
            var key = Encoding.UTF8.GetBytes(configuration["JWT:secret"]);
            options.TokenValidationParameters = new TokenValidationParameters()
            {
                ValidateIssuer = true,
                ValidateAudience = false,
                ValidateLifetime = true,
                IssuerSigningKey = new SymmetricSecurityKey(key)
            };
        });

        // Authorization
        services.AddAuthorization(options => { });

        // RAWG Client
        services.AddHttpClient<IRawgService, RawgService>();
    }
}