using System.Data;
using Dapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;

namespace GamesLand.Tests.Helpers;

public class Table
{
    public string TableName { get; set; }
}

public class IntegrationTestHelper
{
    protected readonly HttpClient Client;
    protected readonly IDbConnection Connection;
    protected readonly IConfiguration Configuration;

    protected IntegrationTestHelper()
    {
        var application = new WebApplicationFactory<Program>()
            .WithWebHostBuilder(builder => { builder.UseEnvironment("test"); });

        Configuration = application.Services.GetRequiredService<IConfiguration>();
        Connection = new NpgsqlConnection()
        {
            ConnectionString = Configuration["pgConnection"]
        };
        Client = application.CreateClient();
    }

    protected async Task DropTables()
    {
        const string tablesQuery = "SELECT table_name FROM information_schema.tables WHERE table_schema = 'public';";
        IEnumerable<Table> tables = await Connection.QueryAsync<Table>(tablesQuery);

        foreach (Table table in tables)
        {
            await Connection.ExecuteAsync($"DROP TABLE IF EXISTS \"{table.TableName}\" CASCADE");
        }
    }
}