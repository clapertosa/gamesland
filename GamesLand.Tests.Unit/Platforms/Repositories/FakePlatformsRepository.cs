using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using GamesLand.Core.Errors;
using GamesLand.Core.Platforms.Entities;
using GamesLand.Core.Platforms.Repositories;

namespace GamesLand.Tests.Unit.Platforms.Repositories;

public class FakePlatformsRepository : IPlatformsRepository
{
    public static Guid RegisteredId => Guid.Parse("3ac9ffce-071a-4b9d-aa5d-c02ad2df8fe2");
    public static int RegisteredExternalId => 4321;

    private Platform GetPlatform(Platform platform) => new Platform()
    {
        Id = platform.Id,
        Name = platform.Name,
        ExternalId = platform.ExternalId,
        GameRequirements = platform.GameRequirements,
        GameReleaseDate = platform.GameReleaseDate,
        CreatedAt = platform.CreatedAt,
        UpdatedAt = platform.UpdatedAt
    };

    public Task<Platform> CreateAsync(Platform entity)
    {
        return Task.FromResult(GetPlatform(new Platform()
        {
            Id = Guid.NewGuid(),
            Name = entity.Name,
            ExternalId = entity.ExternalId
        }));
    }

    public Task<Platform?> GetByIdAsync(Guid id)
    {
        return id == RegisteredId
            ? Task.FromResult<Platform?>(GetPlatform(new Platform() { Id = RegisteredId }))
            : Task.FromResult<Platform?>(null);
    }

    public async Task<IEnumerable<Platform>> GetAllAsync(int page = 0, int pageSize = 10)
    {
        IEnumerable<Task<Platform>> platforms = new List<Task<Platform>>();
        for (int i = 0; i < pageSize; i++)
        {
            platforms.Append(Task.FromResult(GetPlatform(new Platform())));
        }

        return await Task.WhenAll(platforms);
    }

    public Task<Platform> UpdateAsync(Guid id, Platform entity)
    {
        return id == RegisteredId ? Task.FromResult(GetPlatform(entity)) : Task.FromResult<Platform>(null);
    }

    public Task DeleteAsync(Guid id)
    {
        return id == RegisteredId
            ? Task.CompletedTask
            : Task.FromException(new RestException(HttpStatusCode.NotFound, new { Message = "Platform not found." }));
    }

    public async Task<IEnumerable<Platform>> CreateMultipleAsync(IEnumerable<Platform> platforms)
    {
        List<Platform> res = new List<Platform>();
        foreach (Platform platform in platforms)
        {
            if (platform.Id == RegisteredId) res.Add(platform);
        }

        return await Task.FromResult(res);
    }

    public Task<Platform?> GetByExternalIdAsync(int externalId)
    {
        return externalId == RegisteredExternalId
            ? Task.FromResult<Platform?>(GetPlatform(new Platform() { ExternalId = externalId }))
            : Task.FromResult<Platform?>(null);
    }
}