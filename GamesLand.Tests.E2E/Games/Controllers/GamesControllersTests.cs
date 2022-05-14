using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using GamesLand.Core.Users.Entities;
using GamesLand.Infrastructure.RAWG.Entities;
using GamesLand.Infrastructure.RAWG.Requests;
using GamesLand.Tests.Helpers;
using GamesLand.Web.Games.Requests;
using Xunit;

namespace GamesLand.Tests.E2E.Games.Controllers;

public class GamesControllersTests : IntegrationTestBase
{
    [Fact]
    public async Task Search_Games_When_Unauthorized()
    {
        SearchRequest searchRequest = new SearchRequest() { Name = "Red Dead Redemption 2" };
        var res = await Client.GetAsync($"api/games/search?name={searchRequest.Name}");

        Assert.Equal(HttpStatusCode.Unauthorized, res.StatusCode);
    }

    [Fact]
    public async Task Add_Game_To_User()
    {
        User user = new User()
        {
            Email = "email@email.com",
            Password = "password"
        };
        AddGameToUserRequest addGameToUserRequest = new AddGameToUserRequest()
        {
            Game = new RawgGame()
            {
                Name = "Red Dead Redemption",
                Id = 2,
                Slug = "red-dead-redemption",
                Tba = false,
                Rating = 0,
                RatingsCount = 0,
                Updated = "2022-05-14",
                Released = "2022-05-14",
                Platforms = new []{new RawgPlatformParent()
                {
                    Platform = new RawgPlatform()
                    {
                        Id = 1,
                        Name = "PC",
                        Slug = "pc"
                    }
                }}
            },
            Platform = new RawgPlatformParent()
            {
                Platform = new RawgPlatform()
                {
                    Id = 1,
                    Name = "PC",
                    Slug = "pc"
                }
            }
        };

        await Client.PostAsJsonAsync("api/users/signup", user);
        var res = await ClientAuthorized.PostAsJsonAsync("api/games/add", addGameToUserRequest);
        
        Assert.Equal(HttpStatusCode.Created, res.StatusCode);
    }

    [Fact(Skip = "External API quota")]
    public async Task Search_Games_When_Authorized()
    {
        SearchRequest searchRequest = new SearchRequest() { Name = "Red Dead Redemption 2" };
        var res = await ClientAuthorized.GetAsync($"api/games/search?name={searchRequest.Name}");

        Assert.Equal(HttpStatusCode.OK, res.StatusCode);
    }
}