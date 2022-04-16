namespace GamesLand.Tests.Helpers;

public class TestBase : IntegrationTestHelper, IDisposable
{
    public async void Dispose()
    {
        Client.Dispose();
        await DropTables();
    }
}