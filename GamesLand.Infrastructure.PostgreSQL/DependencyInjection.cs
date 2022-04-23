using System.Data;
using Dapper;
using FluentMigrator.Runner;
using GamesLand.Core.Users.Repositories;
using GamesLand.Core.Users.Services;
using GamesLand.Infrastructure.PostgreSQL.Migrations;
using GamesLand.Infrastructure.PostgreSQL.Services;
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
                    .WithGlobalConnectionString(configuration["pgConnection"])
                    .ScanIn(typeof(CreateUuidPgExtension).Assembly).For.Migrations())
            .BuildServiceProvider();

        // PostgreSQL
        DefaultTypeMap.MatchNamesWithUnderscores = true;
        services.AddTransient<IDbConnection>(_ => new NpgsqlConnection(configuration["pgConnection"]));

        // Services
        services.AddSingleton<IUserPasswordService, UserPasswordService>();
        services.AddScoped<IUsersService, UsersService>();

        // Repositories
        services.AddScoped<IUsersRepository, UsersRepository>();
    }
}