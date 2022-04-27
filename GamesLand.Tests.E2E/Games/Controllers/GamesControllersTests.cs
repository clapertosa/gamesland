using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using GamesLand.Infrastructure.RAWG.Requests;
using GamesLand.Tests.Helpers;
using Xunit;

namespace GamesLand.Tests.E2E.Games.Controllers;

public class GamesControllersTests : IntegrationTestBase
{
    [Fact]
    public async Task Search_Games_When_Unauthorized()
    {
        SearchRequest searchRequest = new SearchRequest() { Name = "Red Dead Redemption 2" };
        var res = await Client.PostAsJsonAsync("api/Games/search", searchRequest);

        Assert.Equal(HttpStatusCode.Unauthorized, res.StatusCode);
    }

    [Fact(Skip = "External API quota")]
    public async Task Search_Games_When_Authorized()
    {
        SearchRequest searchRequest = new SearchRequest() { Name = "Red Dead Redemption 2" };
        var res = await ClientAuthorized.PostAsJsonAsync("api/Games/search", searchRequest);

        Assert.Equal(HttpStatusCode.OK, res.StatusCode);
    }
}