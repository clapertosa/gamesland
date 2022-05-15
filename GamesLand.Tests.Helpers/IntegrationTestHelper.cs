using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Dapper;
using GamesLand.Infrastructure.PostgreSQL;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Npgsql;

namespace GamesLand.Tests.Helpers;

public class Table
{
    public string TableName { get; set; }
}

public class IntegrationTestHelper
{
    protected readonly HttpClient Client;
    protected readonly HttpClient ClientAuthorized;
    protected readonly IDbConnection Connection;
    protected readonly IConfiguration Configuration;

    protected IntegrationTestHelper()
    {
        var application = new WebApplicationFactory<Program>()
            .WithWebHostBuilder(builder => { builder.UseEnvironment("test"); });

        Configuration = application.Services.GetRequiredService<IConfiguration>();
        Connection = new NpgsqlConnection()
        {
            ConnectionString = DatabaseUtils.GetDatabaseUrlFormatted(Configuration["DATABASE_URL"])
        };
        Client = application.CreateClient();
        ClientAuthorized = application.CreateClient();
        ClientAuthorized.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", $"Bearer {GetFakeToken()}");
    }

    private string GetFakeToken()
    {
        var claims = new[] { new Claim(ClaimTypes.Email, "email@email.com") };

        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["JWT:secret"]));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);
        var tokenDescriptor = new JwtSecurityToken(null, null, claims,
            expires: DateTime.Now.AddMinutes(10), signingCredentials: credentials);
        return new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
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