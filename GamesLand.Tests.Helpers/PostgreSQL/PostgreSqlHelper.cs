using System.Data;
using Dapper;
using Npgsql;

namespace GamesLand.Tests.Helpers.PostgreSQL;

public class Table
{
    public string TableName { get; set; }
}

public class PostgreSqlHelper : IntegrationTestHelper
{
    protected readonly IDbConnection Connection;

    protected PostgreSqlHelper()
    {
        Connection = new NpgsqlConnection()
        {
            ConnectionString = Configuration["pgConnection"]
        };
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