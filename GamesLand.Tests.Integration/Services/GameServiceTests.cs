using System;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using GamesLand.Core.Games.Entities;
using GamesLand.Core.Games.Services;
using GamesLand.Core.Platforms.Entities;
using GamesLand.Core.Platforms.Services;
using GamesLand.Core.Users.Entities;
using GamesLand.Core.Users.Services;
using GamesLand.Infrastructure.PostgreSQL.Games;
using GamesLand.Infrastructure.PostgreSQL.Platforms;
using GamesLand.Infrastructure.PostgreSQL.Users;
using GamesLand.Tests.Helpers;
using Xunit;

namespace GamesLand.Tests.Integration.Services;

public class GameServiceTests : IntegrationTestBase
{
    private readonly IGamesService _gamesService;

    public GameServiceTests()
    {
        _gamesService = new GamesService(
            new GamesRepository(Connection),
            new PlatformsService(new PlatformsRepository(Connection)),
            new UsersService(new UsersRepository(Connection), new UserAuthentication(Configuration)));
    }

    [Fact]
    public async Task Get_Users_Game_Grouped_By_Game_Id_Successfully()
    {
        var user = new User { FirstName = "Claudio", Email = "email@email.com", Password = "password" };
        var platform = new Platform { Name = "PC", ExternalId = 2 };
        var game = new Game { Name = "Red Dead Redemption", ExternalId = 1 };
        var userRecord = await Connection.QueryFirstAsync<User>(
            $"INSERT INTO users(first_name, email, password) VALUES('{user.FirstName}', '{user.Email}', '{user.Password}') RETURNING *");
        var platformRecord = await Connection.QueryFirstAsync<Platform>(
            $"INSERT INTO platforms(external_id, name) VALUES ({platform.ExternalId}, '{platform.Name}') RETURNING *");
        var gameRecord = await Connection.QueryFirstAsync<Game>(
            $"INSERT INTO games(external_id, name, to_be_announced, updated) VALUES({game.ExternalId}, '{game.Name}', true, current_timestamp) RETURNING *");
        await Connection.ExecuteAsync(
            $"INSERT INTO user_game(user_id, game_id, platform_id, notified, release_date) VALUES('{userRecord.Id}', '{gameRecord.Id}', '{platformRecord.Id}', false, '{new DateOnly()}')");
        var res = await _gamesService.GetUsersGameGroupedByGameIdAsync();

        Assert.Single(res);
    }

    [Fact]
    public async Task Get_Released_Users_Game_Grouped_By_User_Id_Successfully()
    {
        var date = new DateTime(2000, 11, 25);
        var user = new User
            { FirstName = "Claudio", Email = "email@email.com", TelegramChatId = 123, Password = "password" };
        var platform = new Platform { Name = "PC", ExternalId = 2 };
        var game = new Game
        {
            Name = "Red Dead Redemption", ExternalId = 1, Website = "www.reddeadredemption.com", ToBeAnnounced = false
        };
        var userRecord = await Connection.QueryFirstAsync<User>(
            $"INSERT INTO users(first_name, email, telegram_chat_id, password) VALUES('{user.FirstName}', '{user.Email}', '{user.TelegramChatId}', '{user.Password}') RETURNING *");
        var platformRecord = await Connection.QueryFirstAsync<Platform>(
            $"INSERT INTO platforms(external_id, name) VALUES ({platform.ExternalId}, '{platform.Name}') RETURNING *");
        var gameRecord = await Connection.QueryFirstAsync<Game>(
            $"INSERT INTO games(external_id, name, to_be_announced, updated, website) VALUES({game.ExternalId}, '{game.Name}', false, current_timestamp, '{game.Website}') RETURNING *");
        await Connection.ExecuteAsync(
            $"INSERT INTO user_game(user_id, game_id, platform_id, notified, release_date) VALUES('{userRecord.Id}', '{gameRecord.Id}', '{platformRecord.Id}', false, TO_DATE('{date}', 'DD/MM/YYYY'))");
        var res = await _gamesService.GetReleasedUsersGameGroupedByUserIdAsync();
        var record = res.First();

        Assert.NotEqual(Guid.Empty, record.Id);
        Assert.NotEqual(Guid.Empty, record.User.Id);
        Assert.NotEqual(Guid.Empty, record.Platform.Id);
        Assert.Equal(game.Name, record.Name);
        Assert.Equal(user.FirstName, record.User.FirstName);
        Assert.Equal(user.Email, record.User.Email);
        Assert.Equal(user.TelegramChatId, record.User.TelegramChatId);
        Assert.Equal(platform.Name, record.Platform.Name);
        Assert.Equal(date, record.Platform.GameReleaseDate);
    }
}