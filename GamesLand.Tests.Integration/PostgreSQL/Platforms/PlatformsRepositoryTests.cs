﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using GamesLand.Core.Platforms.Entities;
using GamesLand.Core.Platforms.Repositories;
using GamesLand.Infrastructure.PostgreSQL.Platforms;
using GamesLand.Tests.Helpers;
using GamesLand.Tests.Integration.Builders;
using Xunit;

namespace GamesLand.Tests.Integration.PostgreSQL.Platforms;

public class PlatformsRepositoryTests : IntegrationTestBase
{
    private readonly IPlatformsRepository _platformsRepository;

    public PlatformsRepositoryTests()
    {
        _platformsRepository = new PlatformsRepository(Connection);
    }

    [Fact]
    public async Task Create_Platform_With_Valid_Data()
    {
        var platform = new PlatformBuilder().WithExternalId(123).WithName("PC").Build();
        var platformRecord = await _platformsRepository.CreateAsync(platform);

        Assert.Equal(platform.Name, platformRecord.Name);
        Assert.Equal(platform.ExternalId, platformRecord.ExternalId);
    }

    [Fact]
    public async Task Create_Multiple_Platforms_With_Valid_Data()
    {
        IEnumerable<Platform> platforms = new[]
        {
            new PlatformBuilder().WithExternalId(1000).WithName("PC").Build(),
            new PlatformBuilder().WithExternalId(100).WithName("PSX").Build(),
            new PlatformBuilder().WithExternalId(10).WithName("PS10").Build()
        };

        await _platformsRepository.CreateMultipleAsync(platforms);
        const string query = "SELECT COUNT(id) FROM platforms";
        var platformsNumber = await Connection.QueryFirstAsync<int>(query);

        Assert.Equal(platforms.Count(), platformsNumber);
    }

    [Fact]
    public async Task Get_Platform_By_Id()
    {
        var platform = new PlatformBuilder().WithExternalId(123).WithName("PC").Build();
        var platformRecord = await _platformsRepository.CreateAsync(platform);
        var platformRetrieved = await _platformsRepository.GetByIdAsync(platformRecord.Id);

        Assert.NotNull(platformRetrieved);
    }

    [Fact]
    public async Task Get_Platform_By_ExternalId()
    {
        var platform = new PlatformBuilder().WithExternalId(123).WithName("PC").Build();
        var platformRecord = await _platformsRepository.CreateAsync(platform);
        var platformRetrieved = await _platformsRepository.GetByExternalIdAsync(platformRecord.ExternalId);

        Assert.NotNull(platformRetrieved);
    }

    [Fact]
    public async Task Update_Platform()
    {
        var platform = new PlatformBuilder().WithExternalId(123).WithName("PC").Build();
        var platformRecord = await _platformsRepository.CreateAsync(platform);
        var newName = "PSX";
        platform.Name = newName;
        var updatedRecord = await _platformsRepository.UpdateAsync(platformRecord.Id, platform);

        Assert.Equal(newName, updatedRecord.Name);
        Assert.Equal(platformRecord.ExternalId, updatedRecord.ExternalId);
    }

    [Fact]
    public async Task Delete_Platform()
    {
        var platform = new PlatformBuilder().WithExternalId(123).WithName("PC").Build();
        var platformRecord = await _platformsRepository.CreateAsync(platform);
        await _platformsRepository.DeleteAsync(platformRecord.Id);
        var deletedRecord = await _platformsRepository.GetByIdAsync(platformRecord.Id);

        Assert.Null(deletedRecord);
    }
}