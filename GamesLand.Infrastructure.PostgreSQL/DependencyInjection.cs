using System.Data;
using Dapper;
using FluentMigrator.Runner;
using GamesLand.Core.Games.Repositories;
using GamesLand.Core.Games.Services;
using GamesLand.Core.Platforms.Repositories;
using GamesLand.Core.Platforms.Services;
using GamesLand.Core.UserGames.Repositories;
using GamesLand.Core.UserGames.Services;
using GamesLand.Core.Users.Repositories;
using GamesLand.Core.Users.Services;
using GamesLand.Infrastructure.PostgreSQL.Games;
using GamesLand.Infrastructure.PostgreSQL.Migrations;
using GamesLand.Infrastructure.PostgreSQL.Platforms;
using GamesLand.Infrastructure.PostgreSQL.UserGames.Repositories;
using GamesLand.Infrastructure.PostgreSQL.UserGames.Services;
using GamesLand.Infrastructure.PostgreSQL.Users;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;

namespace GamesLand.Infrastructure.PostgreSQL;

public static class DependencyInjection
{
    public static void AddPostgreSqlInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddFluentMigratorCore().ConfigureRunner(rb =>
                rb.AddPostgres()
                    .WithGlobalConnectionString(DatabaseUtils.GetDatabaseUrlFormatted(configuration["DATABASE_URL"]))
                    .ScanIn(typeof(CreateUuidPgExtension).Assembly).For.Migrations())
            .BuildServiceProvider();

        // PostgreSQL
        DefaultTypeMap.MatchNamesWithUnderscores = true;
        services.AddTransient<IDbConnection>(_ =>
            new NpgsqlConnection(DatabaseUtils.GetDatabaseUrlFormatted(configuration["DATABASE_URL"])));

        // Services
        services.AddSingleton<IUserAuthentication, UserAuthentication>();
        services.AddScoped<IUsersService, UsersService>();
        services.AddScoped<IGamesService, GamesService>();
        services.AddScoped<IUserGameService, UserGameService>();
        services.AddScoped<IPlatformsService, PlatformsService>();

        // Repositories
        services.AddScoped<IUsersRepository, UsersRepository>();
        services.AddScoped<IGamesRepository, GamesRepository>();
        services.AddScoped<IUserGameRepository, UserGameRepository>();
        services.AddScoped<IPlatformsRepository, PlatformsRepository>();
    }
}