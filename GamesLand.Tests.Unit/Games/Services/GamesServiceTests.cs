using System;
using System.Net;
using System.Threading.Tasks;
using GamesLand.Core.Errors;
using GamesLand.Core.Games.Entities;
using GamesLand.Core.Games.Services;
using GamesLand.Core.Platforms.Entities;
using GamesLand.Core.Platforms.Services;
using GamesLand.Core.Users.Services;
using GamesLand.Infrastructure.PostgreSQL.Users;
using GamesLand.Tests.Unit.Games.Repositories;
using GamesLand.Tests.Unit.Platforms.Repositories;
using GamesLand.Tests.Unit.Users.Repositories;
using Xunit;

namespace GamesLand.Tests.Unit.Games.Services;

public class GamesServiceTests
{
    private readonly IGamesService _gamesService;

    public GamesServiceTests()
    {
        _gamesService = new GamesService(
            new FakeGamesRepository(),
            new PlatformsService(new FakePlatformsRepository()),
            new UsersService(new FakeUsersRepository(), new UserAuthentication(null))
        );
    }

    [Fact]
    public async Task Create_Game_With_Valid_Data()
    {
        var game = new Game
        {
            Name = "Red Dead Redemption II",
            Description = "description",
            Released = new DateTime(2018, 10, 25),
            Rating = 5.0,
            Website = "https://www.rockstargames.com",
            ToBeAnnounced = false
        };

        var gameRecord = await _gamesService.CreateGameAsync(game);

        Assert.Equal(game.Name, gameRecord.Name);
        Assert.Equal(game.Description, gameRecord.Description);
        Assert.Equal(game.Released.Value.ToLongTimeString(), gameRecord.Released.Value.ToLongTimeString());
        Assert.Equal(game.Website, gameRecord.Website);
        Assert.Equal(game.ToBeAnnounced, gameRecord.ToBeAnnounced);
    }

    [Fact]
    public async Task Get_Game_By_Id_If_Exists()
    {
        var gameRecord = await _gamesService.GetGameByIdAsync(FakeGamesRepository.RegisteredId);
        Assert.Equal(FakeGamesRepository.RegisteredId, gameRecord?.Id);
    }

    [Fact]
    public async Task Get_Game_By_Id_If_Not_Exists()
    {
        var exception =
            await Assert.ThrowsAsync<RestException>(() => _gamesService.GetGameByIdAsync(Guid.NewGuid()));
        Assert.Equal(HttpStatusCode.NotFound, exception.StatusCode);
    }

    [Fact]
    public async Task Get_Game_By_ExternalId_If_Exists()
    {
        var gameRecord = await _gamesService.GetGameByExternalIdAsync(FakeGamesRepository.RegisteredExternalId);
        Assert.Equal(FakeGamesRepository.RegisteredExternalId, gameRecord?.ExternalId);
    }

    [Fact]
    public async Task Get_Game_By_ExternalId_If_Not_Exists()
    {
        var exception =
            await Assert.ThrowsAsync<RestException>(() => _gamesService.GetGameByExternalIdAsync(-1));
        Assert.Equal(HttpStatusCode.NotFound, exception.StatusCode);
    }

    [Fact]
    public async Task Update_Game_If_Exists()
    {
        var gameName = "Update Game";
        var game = await _gamesService.UpdateGameAsync(FakeGamesRepository.RegisteredId,
            new Game { Name = gameName });
        Assert.Equal(gameName, game.Name);
    }

    [Fact]
    public async Task Update_Game_If_Not_Exists()
    {
        var exception =
            await Assert.ThrowsAsync<RestException>(() => _gamesService.UpdateGameAsync(Guid.NewGuid(), new Game()));
        Assert.Equal(HttpStatusCode.NotFound, exception.StatusCode);
    }

    [Fact]
    public async Task Delete_Game_If_Exists()
    {
        try
        {
            await _gamesService.DeleteGameAsync(FakeGamesRepository.RegisteredId);
            Assert.True(true);
        }
        catch (Exception e)
        {
            Assert.True(false);
        }
    }

    [Fact]
    public async Task Delete_Game_If_Not_Exists()
    {
        var exception =
            await Assert.ThrowsAsync<RestException>(() => _gamesService.DeleteGameAsync(Guid.NewGuid()));

        Assert.Equal(HttpStatusCode.NotFound, exception.StatusCode);
    }

    [Fact]
    public async Task Add_Game_To_User_When_User_Exists()
    {
        try
        {
            await _gamesService.AddGameToUserAsync(FakeUsersRepository.RegisteredId,
                new Game
                {
                    Platforms = new[]
                    {
                        new Platform
                        {
                            Id = FakePlatformsRepository.RegisteredId,
                            ExternalId = FakePlatformsRepository.RegisteredExternalId
                        }
                    }
                }, FakePlatformsRepository.RegisteredExternalId);
            Assert.True(true);
        }
        catch
        {
            Assert.True(false);
        }
    }

    [Fact]
    public async Task Add_Game_To_User_When_User_Not_Exists()
    {
        var exception =
            await Assert.ThrowsAsync<RestException>(() => _gamesService.AddGameToUserAsync(Guid.Empty, new Game(), 1));

        Assert.Equal(HttpStatusCode.NotFound, exception.StatusCode);
    }
}