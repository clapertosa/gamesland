using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace GamesLand.Tests.Helpers;

public class IntegrationTestHelper : IDisposable
{
    protected readonly HttpClient Client;
    protected readonly IConfiguration Configuration;

    protected IntegrationTestHelper()
    {
        var application = new WebApplicationFactory<Program>()
            .WithWebHostBuilder(builder => { builder.UseEnvironment("test"); });

        Configuration = application.Services.GetRequiredService<IConfiguration>();
        Client = application.CreateClient();
    }

    public void Dispose()
    {
        Client.Dispose();
    }
}