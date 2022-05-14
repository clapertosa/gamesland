using GamesLand.Core.Platforms.Entities;

namespace GamesLand.Core.Platforms.Services;

public interface IPlatformsService
{
    Task<Platform> CreatePlatformAsync(Platform platform);
    Task<IEnumerable<Platform>> CreateMultiplePlatformsAsync(IEnumerable<Platform> platforms);
    Task<Platform> UpdatePlatformAsync(Guid id, Platform platform);
    Task<Platform?> GetPlatformByIdAsync(Guid id);
    Task<Platform?> GetPlatformByExternalIdAsync(int id);
    Task DeletePlatformAsync(Guid id);
    Task SaveGameReleaseDateAsync(Guid gameId, Guid platformId, DateTime? releaseDate);
}