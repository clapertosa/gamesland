using System.Data;
using Dapper;
using GamesLand.Core.Games.Entities;
using GamesLand.Core.Games.Repositories;
using GamesLand.Core.Platforms.Entities;
using GamesLand.Core.Users.Entities;

namespace GamesLand.Infrastructure.PostgreSQL.Games;

public class GamesRepository : IGamesRepository
{
    private readonly IDbConnection _connection;

    public GamesRepository(IDbConnection connection)
    {
        _connection = connection;
    }

    public async Task<Game> CreateAsync(Game entity)
    {
        const string query =
            "INSERT INTO games(external_id, name, name_original, description, to_be_announced, background_image_path, background_image_additional_path, website, released, updated, rating, ratings_count) VALUES(@ExternalId, @Name, @NameOriginal, @Description, @ToBeAnnounced, @BackgroundImagePath, @BackgroundImageAdditionalPath, @Website, @Released, @Updated, @Rating, @RatingsCount) ON CONFLICT(external_id) DO UPDATE SET name = @Name, name_original = @NameOriginal, description = @Description, to_be_announced = @ToBeAnnounced, background_image_path = @BackgroundImagePath, background_image_additional_path = @BackgroundImageAdditionalPath, website = @Website, released = @Released, updated = @Updated, rating = @Rating, ratings_count = @RatingsCount RETURNING *";
        GamePersistent gamePersistent = await _connection.QuerySingleAsync<GamePersistent>(query, new
        {
            entity.ExternalId,
            entity.Name,
            entity.NameOriginal,
            entity.Description,
            entity.ToBeAnnounced,
            entity.BackgroundImagePath,
            entity.BackgroundImageAdditionalPath,
            entity.Website,
            entity.Released,
            entity.Updated,
            entity.Rating,
            entity.RatingsCount
        });

        return gamePersistent.ToGame();
    }

    public async Task<Game?> GetByIdAsync(Guid id)
    {
        const string query = "SELECT * FROM games WHERE id = @Id";
        GamePersistent gamePersistent =
            await _connection.QueryFirstOrDefaultAsync<GamePersistent>(query, new { Id = id });
        return gamePersistent?.ToGame();
    }

    public async Task<IEnumerable<Game>> GetAllAsync(int page = 0, int pageSize = 10)
    {
        throw new NotImplementedException();
    }

    public async Task<Game> UpdateAsync(Guid id, Game entity)
    {
        const string query =
            "UPDATE games SET name = @Name, name_original = @NameOriginal, description = @Description, to_be_announced = @ToBeAnnounced, background_image_path = @BackgroundImagePath, background_image_additional_path = @BackgroundImageAdditionalPath, website = @Website, released = @Released, updated = @Updated, updated_at = current_timestamp, rating = @Rating, ratings_count = @RatingsCount WHERE id = @Id RETURNING *";
        GamePersistent gamePersistent = await _connection.QueryFirstAsync<GamePersistent>(query, new
        {
            Id = id,
            entity.Name,
            entity.NameOriginal,
            entity.Description,
            entity.ToBeAnnounced,
            entity.BackgroundImagePath,
            entity.BackgroundImageAdditionalPath,
            entity.Website,
            entity.Released,
            entity.Updated,
            entity.Rating,
            entity.RatingsCount
        });
        return gamePersistent.ToGame();
    }

    public Task DeleteAsync(Guid id)
    {
        const string query = "DELETE FROM games WHERE id = @Id";
        return _connection.ExecuteAsync(query, new { Id = id });
    }

    public async Task<Game?> GetByExternalIdAsync(int externalId)
    {
        const string query = "SELECT * FROM games WHERE external_id = @ExternalId";
        GamePersistent gamePersistent =
            await _connection.QueryFirstOrDefaultAsync<GamePersistent>(query, new { ExternalId = externalId });
        return gamePersistent?.ToGame();
    }

    public Task SaveUserGameReleaseDateAsync(Guid userId, Guid gameId, Guid platformId, DateTime? releaseDate)
    {
        const string query =
            "INSERT INTO user_game(user_id, game_id, platform_id, release_date) VALUES(@UserId, @GameId, @PlatformId, @ReleaseDate) ON CONFLICT (user_id, game_id, platform_id) DO UPDATE SET release_date = @ReleaseDate, updated_at = current_timestamp";
        return _connection.ExecuteAsync(query,
            new { UserId = userId, GameId = gameId, PlatformId = platformId, ReleaseDate = releaseDate });
    }

    public Task RemoveGameFromUserAsync(Guid userId, Guid gameId, int platformId)
    {
        const string query =
            "DELETE FROM user_game WHERE user_id = @UserId AND game_id = @GameId AND platform_id = @PlatformId";
        return _connection.ExecuteAsync(query, new { UserId = userId, GameId = gameId, PlatformId = platformId });
    }

    public async Task<IEnumerable<Game>> GetUsersGameGroupedByGameIdAsync()
    {
        const string query =
            "SELECT G.id, G.external_id, G.name FROM games G JOIN user_game UG ON G.id = UG.game_id JOIN users U ON U.id = UG.user_id WHERE UG.notified = false GROUP BY G.id";
        IEnumerable<GamePersistent> gamePersistent = await _connection.QueryAsync<GamePersistent>(query);
        return gamePersistent.Select(x => x.ToGame());
    }

    public Task<IEnumerable<Game>> GetReleasedUsersGameGroupedByUserIdAsync()
    {
        const string query =
            @"SELECT 
            G.id AS game_id, G.name, G.background_image_path, G.website, 
            U.id AS user_id, U.first_name, U.email, 
            P.id AS platform_id, P.name, 
            UG.release_date AS game_release_date
            FROM games G
                     JOIN user_game UG ON G.id = UG.game_id
                     JOIN users U ON U.id = UG.user_id
                     JOIN platforms P ON P.id = UG.platform_id
            WHERE UG.notified = FALSE
              AND UG.release_date <= CURRENT_TIMESTAMP
            GROUP BY G.id, G.name, G.background_image_path, G.website, U.id, P.id, P.name, UG.release_date";
        return _connection.QueryAsync<Game, User, Platform, Game>(query, (g, u, p) =>
        {
            Game game = g;
            g.User = u;
            g.Platform = p;
            return game;
        }, splitOn: "user_id, platform_id");
    }
}