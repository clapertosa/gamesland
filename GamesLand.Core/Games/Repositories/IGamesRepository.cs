using GamesLand.Core.Games.Entities;

namespace GamesLand.Core.Games.Repositories;

public interface IGamesRepository : IRepository<Guid, Game>
{
    Task<Game?> GetByExternalIdAsync(int externalId);
    Task SaveUserGameReleaseDateAsync(Guid userId, Guid gameId, Guid platformId, DateTime? releaseDate);
    Task RemoveGameFromUserAsync(Guid userId, Guid gameId, int platformId);
    Task<IEnumerable<Game>> GetUsersGameGroupedByGameIdAsync();
    Task<IEnumerable<Game>> GetUsersGameGroupedByUserIdAsync();
}