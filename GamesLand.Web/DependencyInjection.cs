using System.Text;
using FluentValidation.AspNetCore;
using GamesLand.Infrastructure.PostgreSQL;
using GamesLand.Infrastructure.RAWG.Handlers;
using GamesLand.Infrastructure.RAWG.Services;
using GamesLand.Infrastructure.Scheduler.Jobs;
using GamesLand.Infrastructure.Telegram;
using GamesLand.Web.Users.Validators;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Serialization;
using Quartz;
using Telegram.Bot;

namespace GamesLand.Web;

public static class DependencyInjection
{
    public static void AddServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddFluentValidation(fv => fv.RegisterValidatorsFromAssembly(typeof(SignUpUserValidator).Assembly));
        services.AddControllers()
            .AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.ContractResolver = new DefaultContractResolver
                {
                    NamingStrategy = new CamelCaseNamingStrategy()
                };
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
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = true,
                IssuerSigningKey = new SymmetricSecurityKey(key)
            };
        });

        // Authorization
        services.AddAuthorization();

        // Telegram
        services.AddTelegram();
        services.AddHttpClient("telegram").AddTypedClient<ITelegramBotClient>(client =>
        {
            client.BaseAddress = new Uri("https://api.telegram.org");
            return new TelegramBotClient(configuration["telegram_bot:token"], client);
        });

        // Quartz
        // Every day at 00 AM: 0 0 0 * * ?
        // Every day at 01 AM: 0 0 1 * * ?
        // 5 seconds: 0/5 * * * * ?
        services.AddQuartz(q =>
        {
            q.UseMicrosoftDependencyInjectionJobFactory();
            var sendReleasedGamesMessageJobKey = new JobKey("SendReleasedGamesMessageJob");
            q.AddJob<SendReleasedGamesMessageJob>(config => config
                .WithIdentity(sendReleasedGamesMessageJobKey));
            q.AddTrigger(config => config
                .ForJob(sendReleasedGamesMessageJobKey)
                .WithCronSchedule("0 0 0 * * ?")); // 00 AM UTC

            var deleteNotifiedGamesJobKey = new JobKey("DeleteNotifiedGamesJob");
            q.AddJob<DeleteNotifiedGamesJob>(config => config
                .WithIdentity(deleteNotifiedGamesJobKey));
            q.AddTrigger(config => config
                .ForJob(deleteNotifiedGamesJobKey)
                .WithCronSchedule("0 0 1 * * ?")); // 01 AM UTC
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