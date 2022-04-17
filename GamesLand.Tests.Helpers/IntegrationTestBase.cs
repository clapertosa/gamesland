using Xunit;

namespace GamesLand.Tests.Helpers;

public class IntegrationTestBase : IntegrationTestHelper, IAsyncLifetime
{
    public async Task InitializeAsync()
    {
    }

    async Task IAsyncLifetime.DisposeAsync()
    {
        Client.Dispose();
        await DropTables();
    }
}