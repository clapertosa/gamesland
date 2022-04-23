using System.Data;
using Dapper;
using GamesLand.Core.Games.Entities;
using GamesLand.Core.Games.Repositories;

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
            @"INSERT INTO games(external_id, slug, name, name_original, description, to_be_announced, background_image_path, background_image_additional_path, website, released, updated, rating, ratings_count) VALUES(@ExternalId, @Slug, @Name, @NameOriginal, @Description, @ToBeAnnounced, @BackgroundImagePath, @BackgroundImageAdditionalPath, @Website, @Released, @Updated, @Rating, @RatingsCount) RETURNING *";
        GamePersistent gamePersistent = await _connection.QuerySingleAsync<GamePersistent>(query, new
        {
            entity.ExternalId,
            entity.Slug,
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
            entity.Slug,
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

    public async Task<Game?> GetByExternalIdAsync(int id)
    {
        const string query = "SELECT * FROM games WHERE external_id = @ExternalId";
        GamePersistent gamePersistent =
            await _connection.QueryFirstOrDefaultAsync<GamePersistent>(query, new { ExternalId = id });
        return gamePersistent?.ToGame();
    }
}