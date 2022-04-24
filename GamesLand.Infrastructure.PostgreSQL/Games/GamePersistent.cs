using GamesLand.Core;
using GamesLand.Core.Games.Entities;

namespace GamesLand.Infrastructure.PostgreSQL.Games;

public record GamePersistent : BaseEntity<Guid>
{
    public int ExternalId { get; set; }
    public string Name { get; set; }
    public string NameOriginal { get; set; }
    public string Description { get; set; }
    public DateTime Released { get; set; }
    public DateTime Updated { get; set; }
    public bool ToBeAnnounced { get; set; }
    public string BackgroundImagePath { get; set; }
    public string BackgroundImageAdditionalPath { get; set; }
    public string Website { get; set; }
    public double Rating { get; set; }
    public int RatingsCount { get; set; }

    public Game ToGame() => new Game()
    {
        Id = Id,
        ExternalId = ExternalId,
        Name = Name,
        NameOriginal = NameOriginal,
        Description = Description,
        Released = Released,
        Updated = Updated,
        ToBeAnnounced = ToBeAnnounced,
        BackgroundImagePath = BackgroundImagePath,
        BackgroundImageAdditionalPath = BackgroundImageAdditionalPath,
        Website = Website,
        Rating = Rating,
        RatingsCount = RatingsCount,
        CreatedAt = CreatedAt,
        UpdatedAt = UpdatedAt
    };
}