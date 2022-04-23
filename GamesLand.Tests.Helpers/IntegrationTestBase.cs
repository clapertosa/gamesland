using Xunit;

namespace GamesLand.Tests.Helpers;

public class IntegrationTestBase : IntegrationTestHelper, IAsyncLifetime
{
    public Task InitializeAsync()
    {
        return Task.CompletedTask;
    }

    async Task IAsyncLifetime.DisposeAsync()
    {
        Client.Dispose();
        await DropTables();
    }
}