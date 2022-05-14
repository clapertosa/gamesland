using GamesLand.Core.Platforms.Entities;

namespace GamesLand.Core.Games.Entities;

public record Game : BaseEntity<Guid>
{
    public int ExternalId { get; set; }
    public string Name { get; set; }
    public string? NameOriginal { get; set; }
    public string Description { get; set; }
    public DateTime? Released { get; set; }
    public DateTime? Updated { get; set; }
    public bool? ToBeAnnounced { get; set; }
    public string? BackgroundImagePath { get; set; }
    public string? BackgroundImageAdditionalPath { get; set; }
    public string? Website { get; set; }
    public double? Rating { get; set; }
    public int? RatingsCount { get; set; }
    public IEnumerable<Platform>? Platforms { get; set; }
}