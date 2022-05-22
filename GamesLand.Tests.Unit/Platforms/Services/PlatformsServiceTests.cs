using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using GamesLand.Core.Errors;
using GamesLand.Core.Platforms.Entities;
using GamesLand.Core.Platforms.Services;
using GamesLand.Tests.Unit.Platforms.Repositories;
using Xunit;

namespace GamesLand.Tests.Unit.Platforms.Services;

public class PlatformsServiceTests
{
    private readonly IPlatformsService _platformsService;

    public PlatformsServiceTests()
    {
        _platformsService = new PlatformsService(new FakePlatformsRepository());
    }

    [Fact]
    public async Task Create_Platform_With_Valid_Data()
    {
        var platform = new Platform { ExternalId = 123, Name = "PC" };
        var platformRecord = await _platformsService.CreatePlatformAsync(platform);

        Assert.Equal(platform.Name, platformRecord.Name);
        Assert.Equal(platform.ExternalId, platformRecord.ExternalId);
    }

    [Fact]
    public async Task Create_Multiple_Platforms_With_Valid_Data()
    {
        IEnumerable<Platform> platforms = new[]
        {
            new Platform { Id = FakePlatformsRepository.RegisteredId },
            new Platform { Id = FakePlatformsRepository.RegisteredId },
            new Platform { Id = FakePlatformsRepository.RegisteredId }
        };

        var platformsRecord = await _platformsService.CreateMultiplePlatformsAsync(platforms);

        Assert.Equal(platforms.Count(), platformsRecord.Count());
    }

    [Fact]
    public async Task Get_Platform_By_Id_When_Exists()
    {
        var platformRecord = await _platformsService.GetPlatformByIdAsync(FakePlatformsRepository.RegisteredId);
        Assert.Equal(FakePlatformsRepository.RegisteredId, platformRecord?.Id);
    }

    [Fact]
    public async Task Get_Platform_By_Id_When_Not_Exists()
    {
        var exception =
            await Assert.ThrowsAsync<RestException>(() => _platformsService.GetPlatformByIdAsync(Guid.NewGuid()));

        Assert.Equal(HttpStatusCode.NotFound, exception.StatusCode);
    }

    [Fact]
    public async Task Get_Platform_By_ExternalId_When_Exists()
    {
        var platformRecord =
            await _platformsService.GetPlatformByExternalIdAsync(FakePlatformsRepository.RegisteredExternalId);

        Assert.Equal(FakePlatformsRepository.RegisteredExternalId, platformRecord?.ExternalId);
    }

    [Fact]
    public async Task Get_Platform_By_ExternalId_When_Not_Exists()
    {
        var exception =
            await Assert.ThrowsAsync<RestException>(() => _platformsService.GetPlatformByExternalIdAsync(-1));

        Assert.Equal(HttpStatusCode.NotFound, exception.StatusCode);
    }

    [Fact]
    public async Task Update_Game_When_Exists()
    {
        var platformRecord =
            await _platformsService.UpdatePlatformAsync(FakePlatformsRepository.RegisteredId,
                new Platform { Name = "PC" });

        Assert.Equal("PC", platformRecord.Name);
    }

    [Fact]
    public async Task Update_Game_When_Not_Exists()
    {
        var exception =
            await Assert.ThrowsAsync<RestException>(() =>
                _platformsService.UpdatePlatformAsync(Guid.NewGuid(), new Platform()));

        Assert.Equal(HttpStatusCode.NotFound, exception.StatusCode);
    }

    [Fact]
    public async Task Delete_Platform_If_Exists()
    {
        try
        {
            await _platformsService.DeletePlatformAsync(FakePlatformsRepository.RegisteredId);
            Assert.True(true);
        }
        catch (Exception e)
        {
            Assert.True(false);
        }
    }

    [Fact]
    public async Task Delete_Platform_If_Not_Exists()
    {
        var exception =
            await Assert.ThrowsAsync<RestException>(() => _platformsService.DeletePlatformAsync(Guid.NewGuid()));

        Assert.Equal(HttpStatusCode.NotFound, exception.StatusCode);
    }
}