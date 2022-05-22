using System;
using System.Threading.Tasks;
using Dapper;
using GamesLand.Core.Games.Repositories;
using GamesLand.Infrastructure.PostgreSQL.Games;
using GamesLand.Tests.Helpers;
using GamesLand.Tests.Integration.Builders;
using Xunit;

namespace GamesLand.Tests.Integration.PostgreSQL.Games;

public class GamesRepositoryTests : IntegrationTestBase
{
    private readonly IGamesRepository _gamesRepository;

    public GamesRepositoryTests()
    {
        _gamesRepository = new GamesRepository(Connection);
    }

    [Fact]
    public async Task Create_Game_With_Valid_Data()
    {
        var game = new GameBuilder()
            .WithExternalId(1233)
            .WithName("Red Dead Redemption")
            .WithDescription("Western")
            .WithRating(5.0)
            .WithToBeAnnounced(true)
            .WithRatingsCount(0)
            .WithUpdated(DateTime.Now)
            .WithRating(0)
            .Build();

        var gameRecord = await _gamesRepository.CreateAsync(game);

        Assert.Equal(game.Name, gameRecord.Name);
        Assert.Equal(game.Description, gameRecord.Description);
        Assert.Equal(game.Rating, gameRecord.Rating);
        Assert.Equal(game.ExternalId, gameRecord.ExternalId);
    }

    [Fact]
    public async Task Get_Game_By_Id()
    {
        var game = new GameBuilder()
            .WithName("Name")
            .WithExternalId(123)
            .WithToBeAnnounced(true)
            .WithRatingsCount(0)
            .WithUpdated(DateTime.Now)
            .WithRating(0)
            .Build();

        var gameRecord = await _gamesRepository.CreateAsync(game);
        var returnedGame = await _gamesRepository.GetByIdAsync(gameRecord.Id);

        Assert.Equal(gameRecord.Id, returnedGame?.Id);
        Assert.Equal(gameRecord.ExternalId, returnedGame?.ExternalId);
    }

    [Fact]
    public async Task Get_Game_By_ExternalId()
    {
        var game = new GameBuilder()
            .WithName("Name")
            .WithExternalId(123)
            .WithToBeAnnounced(true)
            .WithRatingsCount(0)
            .WithUpdated(DateTime.Now)
            .WithRating(0)
            .Build();

        await _gamesRepository.CreateAsync(game);
        var gameRecord = await _gamesRepository.GetByExternalIdAsync(game.ExternalId);

        Assert.Equal(game.ExternalId, gameRecord?.ExternalId);
    }

    [Fact]
    public async Task Update_Game_With_Valid_Data()
    {
        var game = new GameBuilder()
            .WithName("Name")
            .WithExternalId(123)
            .WithToBeAnnounced(true)
            .WithRatingsCount(0)
            .WithUpdated(DateTime.Now)
            .WithRating(0)
            .Build();

        var newName = "new name";

        var oldGameRecord = await _gamesRepository.CreateAsync(game);
        game.Name = newName;
        await _gamesRepository.UpdateAsync(oldGameRecord.Id, game);
        var newGameRecord = await _gamesRepository.GetByIdAsync(oldGameRecord.Id);

        Assert.Equal(newName, newGameRecord?.Name);
    }

    [Fact]
    public async Task Delete_Game_Which_Exists()
    {
        var game = new GameBuilder()
            .WithName("Name")
            .WithExternalId(123)
            .WithToBeAnnounced(true)
            .WithRatingsCount(0)
            .WithUpdated(DateTime.Now)
            .WithRating(0)
            .Build();

        var gameRecord = await _gamesRepository.CreateAsync(game);
        await _gamesRepository.DeleteAsync(gameRecord.Id);
        var gameExists = await _gamesRepository.GetByIdAsync(gameRecord.Id) != null;

        Assert.False(gameExists);
    }

    [Fact]
    public async Task Create_Existing_Game_Updates_It()
    {
        var game = new GameBuilder()
            .WithName("Castlevania")
            .WithExternalId(123)
            .WithDescription("Symphony of the Night")
            .WithRating(5)
            .WithReleased(DateTime.UtcNow)
            .WithWebsite("www.www.com")
            .WithToBeAnnounced(true)
            .WithRatingsCount(0)
            .WithUpdated(DateTime.Now)
            .WithRating(0)
            .Build();

        await _gamesRepository.CreateAsync(game);
        const string name = "Castlevania - Symphony of The Night";
        game.NameOriginal = name;
        var gameRecord = await _gamesRepository.CreateAsync(game);

        const string query = "SELECT COUNT(id) FROM games";
        var res = await Connection.QueryFirstAsync<int>(query);

        Assert.Equal(name, gameRecord.NameOriginal);
        Assert.Equal(1, res);
    }
}