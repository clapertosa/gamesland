using System;
using GamesLand.Core.Platforms.Entities;

namespace GamesLand.Tests.Integration.Builders;

public class PlatformBuilder
{
    private readonly Platform _platform = new Platform();

    public PlatformBuilder()
    {
    }

    public PlatformBuilder WithId(Guid id)
    {
        _platform.Id = id;
        return this;
    }
    
    public PlatformBuilder WithExternalId(int id)
    {
        _platform.ExternalId = id;
        return this;
    }
    
    public PlatformBuilder WithName(string name)
    {
        _platform.Name = name;
        return this;
    }
    
    public PlatformBuilder WithGameRequirements(string gameRequirements)
    {
        _platform.GameRequirements = gameRequirements;
        return this;
    }

    public PlatformBuilder WithGameReleaseDate(DateTime releaseDate)
    {
        _platform.GameReleaseDate = releaseDate;
        return this;
    }

    public Platform Build() => _platform;
}