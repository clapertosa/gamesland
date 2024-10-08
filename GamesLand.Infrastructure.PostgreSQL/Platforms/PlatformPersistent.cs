﻿using GamesLand.Core;
using GamesLand.Core.Platforms.Entities;

namespace GamesLand.Infrastructure.PostgreSQL.Platforms;

internal record PlatformPersistent : BaseEntity<Guid>
{
    public int ExternalId { get; set; }
    public string Name { get; set; }

    public Platform ToPlatform(DateTime? gameReleaseDate = null, string? requirements = null)
    {
        return new Platform
        {
            Id = Id,
            ExternalId = ExternalId,
            Name = Name,
            GameReleaseDate = gameReleaseDate,
            GameRequirements = requirements,
            CreatedAt = CreatedAt,
            UpdatedAt = UpdatedAt
        };
    }
}