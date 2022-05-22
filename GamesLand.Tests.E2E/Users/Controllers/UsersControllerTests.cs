using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using GamesLand.Core.Users.Entities;
using GamesLand.Tests.Helpers;
using GamesLand.Web.Users.Responses;
using Xunit;

namespace GamesLand.Tests.E2E.Users.Controllers;

public class UsersControllerTests : IntegrationTestBase
{
    [Fact]
    public async Task Sign_Up_User_With_Valid_Data()
    {
        var user = new User
        {
            FirstName = "Name",
            LastName = "Last name",
            Email = "email@email.com",
            Password = "password",
            TelegramChatId = 123
        };

        var res = await Client.PostAsJsonAsync("api/users/signup", user);
        var userResponse = await res.Content.ReadFromJsonAsync<UserResponse>();

        Assert.Equal(HttpStatusCode.Created, res.StatusCode);
        Assert.Equal(user.FirstName, userResponse?.FirstName);
        Assert.Equal(user.LastName, userResponse?.LastName);
        Assert.Equal(user.Email, userResponse?.Email);
        Assert.Equal(user.TelegramChatId, userResponse?.TelegramChatId);
    }

    [Fact]
    public async Task Sign_Up_User_With_A_Not_Valid_Email()
    {
        var user = new User
        {
            FirstName = "Name",
            LastName = "Last name",
            Email = "123",
            Password = "password"
        };

        var res = await Client.PostAsJsonAsync("api/users/signup", user);

        Assert.Equal(HttpStatusCode.BadRequest, res.StatusCode);
    }

    [Fact]
    public async Task Sign_Up_User_Without_An_Email()
    {
        var user = new User
        {
            FirstName = "Name",
            LastName = "Last name",
            Password = "password"
        };

        var res = await Client.PostAsJsonAsync("api/users/signup", user);

        Assert.Equal(HttpStatusCode.BadRequest, res.StatusCode);
    }

    [Fact]
    public async Task Sign_Up_User_With_A_Not_Valid_Password()
    {
        var user = new User
        {
            FirstName = "Name",
            LastName = "Last name",
            Email = "email@email.com",
            Password = "pass"
        };

        var res = await Client.PostAsJsonAsync("api/users/signup", user);

        Assert.Equal(HttpStatusCode.BadRequest, res.StatusCode);
    }

    [Fact]
    public async Task Sign_Up_User_Without_A_Password()
    {
        var user = new User
        {
            FirstName = "Name",
            LastName = "Last name",
            Email = "email@email.com"
        };

        var res = await Client.PostAsJsonAsync("api/users/signup", user);

        Assert.Equal(HttpStatusCode.BadRequest, res.StatusCode);
    }

    [Fact]
    public async Task Sign_In_User_With_Valid_Data()
    {
        var user = new User
        {
            FirstName = "Name",
            LastName = "Last name",
            Email = "email@email.com",
            Password = "password"
        };

        await Client.PostAsJsonAsync("api/users/signup", user);
        var res = await Client.PostAsJsonAsync("api/users/signin", user);
        var token = await res.Content.ReadAsStringAsync();

        Assert.NotNull(token);
    }
}