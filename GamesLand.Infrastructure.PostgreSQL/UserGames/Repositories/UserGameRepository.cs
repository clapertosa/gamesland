using System.Data;
using Dapper;
using GamesLand.Core.UserGames.Entities;
using GamesLand.Core.UserGames.Repositories;

namespace GamesLand.Infrastructure.PostgreSQL.UserGames.Repositories;

public class UserGameRepository : IUserGameRepository
{
    private readonly IDbConnection _connection;

    public UserGameRepository(IDbConnection connection)
    {
        _connection = connection;
    }

    public Task<UserGame> UpdateReleaseDateAsync(Guid gameId, Guid platformId, DateTime releaseDate)
    {
        const string query =
            "UPDATE user_game SET release_date = @ReleaseDate WHERE game_id = @GameId AND platform_id = @PlatformId RETURNING *";

        return _connection.QueryFirstAsync<UserGame>(query,
            new { ReleaseDate = releaseDate, GameId = gameId, PlatformId = platformId });
    }
}