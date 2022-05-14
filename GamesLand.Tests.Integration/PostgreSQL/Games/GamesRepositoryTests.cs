using System;
using System.Threading.Tasks;
using Dapper;
using GamesLand.Core.Games.Entities;
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
        Game game = new GameBuilder()
            .WithExternalId(1233)
            .WithName("Red Dead Redemption")
            .WithDescription("Western")
            .WithRating(5.0)
            .WithToBeAnnounced(true)
            .WithRatingsCount(0)
            .WithUpdated(DateTime.Now)
            .WithRating(0)
            .Build();

        Game gameRecord = await _gamesRepository.CreateAsync(game);

        Assert.Equal(game.Name, gameRecord.Name);
        Assert.Equal(game.Description, gameRecord.Description);
        Assert.Equal(game.Rating, gameRecord.Rating);
        Assert.Equal(game.ExternalId, gameRecord.ExternalId);
    }

    [Fact]
    public async Task Get_Game_By_Id()
    {
        Game game = new GameBuilder()
            .WithName("Name")
            .WithExternalId(123)
            .WithToBeAnnounced(true)
            .WithRatingsCount(0)
            .WithUpdated(DateTime.Now)
            .WithRating(0)
            .Build();

        Game gameRecord = await _gamesRepository.CreateAsync(game);
        Game? returnedGame = await _gamesRepository.GetByIdAsync(gameRecord.Id);

        Assert.Equal(gameRecord.Id, returnedGame?.Id);
        Assert.Equal(gameRecord.ExternalId, returnedGame?.ExternalId);
    }

    [Fact]
    public async Task Get_Game_By_ExternalId()
    {
        Game game = new GameBuilder()
            .WithName("Name")
            .WithExternalId(123)
            .WithToBeAnnounced(true)
            .WithRatingsCount(0)
            .WithUpdated(DateTime.Now)
            .WithRating(0)
            .Build();

        await _gamesRepository.CreateAsync(game);
        Game? gameRecord = await _gamesRepository.GetByExternalIdAsync(game.ExternalId);

        Assert.Equal(game.ExternalId, gameRecord?.ExternalId);
    }

    [Fact]
    public async Task Update_Game_With_Valid_Data()
    {
        Game game = new GameBuilder()
            .WithName("Name")
            .WithExternalId(123)
            .WithToBeAnnounced(true)
            .WithRatingsCount(0)
            .WithUpdated(DateTime.Now)
            .WithRating(0)
            .Build();

        string newName = "new name";

        Game? oldGameRecord = await _gamesRepository.CreateAsync(game);
        game.Name = newName;
        await _gamesRepository.UpdateAsync(oldGameRecord.Id, game);
        Game? newGameRecord = await _gamesRepository.GetByIdAsync(oldGameRecord.Id);

        Assert.Equal(newName, newGameRecord?.Name);
    }

    [Fact]
    public async Task Delete_Game_Which_Exists()
    {
        Game game = new GameBuilder()
            .WithName("Name")
            .WithExternalId(123)
            .WithToBeAnnounced(true)
            .WithRatingsCount(0)
            .WithUpdated(DateTime.Now)
            .WithRating(0)
            .Build();

        Game? gameRecord = await _gamesRepository.CreateAsync(game);
        await _gamesRepository.DeleteAsync(gameRecord.Id);
        bool gameExists = await _gamesRepository.GetByIdAsync(gameRecord.Id) != null;

        Assert.False(gameExists);
    }

    [Fact]
    public async Task Create_Existing_Game_Updates_It()
    {
        Game game = new GameBuilder()
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
        Game gameRecord = await _gamesRepository.CreateAsync(game);

        const string query = "SELECT COUNT(id) FROM games";
        int res = await Connection.QueryFirstAsync<int>(query);

        Assert.Equal(name, gameRecord.NameOriginal);
        Assert.Equal(1, res);
    }
}