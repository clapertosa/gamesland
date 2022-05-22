using GamesLand.Core.Games.Entities;
using GamesLand.Core.Platforms.Entities;
using GamesLand.Infrastructure.RAWG.Entities;

namespace GamesLand.Web.Games.Requests;

public record AddGameToUserRequest
{
    public RawgGame Game { get; set; }
    public RawgPlatformParent Platform { get; set; }

    public Game ToGame()
    {
        return new()
        {
            ExternalId = Game.Id,
            Name = Game.Name,
            NameOriginal = Game.NameOriginal,
            Description = Game.Description ?? "",
            Rating = Game.Rating,
            Released = Game.Released != null ? DateTime.Parse(Game.Released) : null,
            ToBeAnnounced = Game.Tba,
            Updated = Game.Updated != null ? DateTime.Parse(Game.Updated) : null,
            Website = Game.Website,
            RatingsCount = Game.RatingsCount,
            BackgroundImagePath = Game.BackgroundImage,
            BackgroundImageAdditionalPath = Game.BackgroundImageAdditional,
            Platforms = Game.Platforms?.Select(x => new Platform
            {
                Name = x.Platform.Name,
                ExternalId = x.Platform.Id,
                GameReleaseDate = x.ReleasedAt,
                GameRequirements = $"Minimum: {x.Requirements?.Minimum}/Recommended: {x.Requirements?.Recommended}"
            })
        };
    }

    public Platform ToPlatform()
    {
        return new()
        {
            ExternalId = Platform.Platform.Id,
            Name = Platform.Platform.Name,
            GameReleaseDate = Platform.ReleasedAt,
            GameRequirements =
                $"Minimum: {Platform.Requirements?.Minimum}/Recommended: {Platform.Requirements?.Recommended}"
        };
    }
}