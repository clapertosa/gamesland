using GamesLand.Core.Games.Entities;

namespace GamesLand.Core.Games.Services;

public interface IGamesService
{
    Task<Game> CreateGameAsync(Game game);
    Task<Game?> GetGameByIdAsync(Guid id);
    Task<Game?> GetGameByExternalIdAsync(int id);
    Task<Game> UpdateGameAsync(Guid id, Game game);
    Task DeleteGameAsync(Guid id);
    Task AddGameToUserAsync(Guid userId, Game game, int platformId);
    Task<IEnumerable<Game>> GetUsersGameGroupedByGameIdAsync();
    Task<IEnumerable<Game>> GetUsersGameGroupedByUserIdAsync();
}