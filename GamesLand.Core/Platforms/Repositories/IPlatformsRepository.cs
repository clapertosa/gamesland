using GamesLand.Core.Platforms.Entities;

namespace GamesLand.Core.Platforms.Repositories;

public interface IPlatformsRepository : IRepository<Guid, Platform>
{
    Task<IEnumerable<Platform>> CreateMultipleAsync(IEnumerable<Platform> platforms);
    Task<Platform?> GetByExternalIdAsync(int externalId);
    Task SaveGameReleaseDateAsync(Guid gameId, Guid platformId, DateTime? releaseDate);
}