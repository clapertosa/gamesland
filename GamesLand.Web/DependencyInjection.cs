using System.Text;
using System.Text.Json.Serialization;
using FluentValidation.AspNetCore;
using GamesLand.Infrastructure.MailSender.Services;
using GamesLand.Infrastructure.PostgreSQL;
using GamesLand.Infrastructure.RAWG.Handlers;
using GamesLand.Infrastructure.RAWG.Services;
using GamesLand.Infrastructure.Scheduler.Jobs;
using GamesLand.Web.Users.Validators;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Quartz;

namespace GamesLand.Web;

public static class DependencyInjection
{
    public static void AddServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddFluentValidation(fv => fv.RegisterValidatorsFromAssembly(typeof(SignUpUserValidator).Assembly));
        services.AddControllers()
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
                options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault;
            });
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
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = true,
                IssuerSigningKey = new SymmetricSecurityKey(key)
            };
        });

        // Authorization
        services.AddAuthorization();

        // Mail
        services.AddSingleton<IMailService, MailService>();

        // Quartz
        services.AddQuartz(q =>
        {
            q.UseMicrosoftDependencyInjectionJobFactory();
            JobKey sendReleasedGamesMailJobKey = new JobKey("SendReleasedGamesMailJob");
            q.AddJob<SendReleasedGamesMailJob>(config => config
                .WithIdentity(sendReleasedGamesMailJobKey));
            q.AddTrigger(config => config
                .ForJob(sendReleasedGamesMailJobKey)
                .WithCronSchedule("0 0 0 * * ?")); // 00 AM

            JobKey deleteNotifiedGamesJobKey = new JobKey("DeleteNotifiedGamesJob");
            q.AddJob<DeleteNotifiedGamesJob>(config => config
                .WithIdentity(deleteNotifiedGamesJobKey));
            q.AddTrigger(config => config
                .ForJob(deleteNotifiedGamesJobKey)
                .WithCronSchedule("0 0 1 * * ?")); // 01 AM
        });
        services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);

        // RAWG Client
        services.AddTransient<RawgHttpMessageHandler>();
        services
            .AddHttpClient<IRawgService, RawgService>("RAWG-Client",
                client => { client.BaseAddress = new Uri("https://api.rawg.io/api/"); })
            .AddHttpMessageHandler<RawgHttpMessageHandler>();
    }
}