using System.Net;
using GamesLand.Core.Errors;
using GamesLand.Core.Platforms.Entities;
using GamesLand.Core.Platforms.Repositories;

namespace GamesLand.Core.Platforms.Services;

public class PlatformsService : IPlatformsService
{
    private readonly IPlatformsRepository _platformsRepository;

    public PlatformsService(IPlatformsRepository platformsRepository)
    {
        _platformsRepository = platformsRepository;
    }

    public Task<Platform> CreatePlatformAsync(Platform platform)
    {
        return _platformsRepository.CreateAsync(platform);
    }

    public Task<IEnumerable<Platform>> CreateMultiplePlatformsAsync(IEnumerable<Platform> platforms)
    {
        return _platformsRepository.CreateMultipleAsync(platforms);
    }

    public async Task<Platform> UpdatePlatformAsync(Guid id, Platform platform)
    {
        var platformRecord = await _platformsRepository.GetByIdAsync(id);
        if (platformRecord == null)
            throw new RestException(HttpStatusCode.NotFound, new { Message = "Platform not found." });
        return await _platformsRepository.UpdateAsync(id, platform);
    }

    public async Task<Platform?> GetPlatformByIdAsync(Guid id)
    {
        var platformRecord = await _platformsRepository.GetByIdAsync(id);
        if (platformRecord == null)
            throw new RestException(HttpStatusCode.NotFound, new { Message = "Platform not found." });
        return platformRecord;
    }

    public async Task<Platform?> GetPlatformByExternalIdAsync(int id)
    {
        var platformRecord = await _platformsRepository.GetByExternalIdAsync(id);
        if (platformRecord == null)
            throw new RestException(HttpStatusCode.NotFound, new { Message = "Platform not found." });
        return platformRecord;
    }

    public async Task DeletePlatformAsync(Guid id)
    {
        var platformRecord = await _platformsRepository.GetByIdAsync(id);
        if (platformRecord == null)
            throw new RestException(HttpStatusCode.NotFound, new { Message = "Platform not found." });
        await _platformsRepository.DeleteAsync(id);
    }
}