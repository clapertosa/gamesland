namespace GamesLand.Core.Platforms.Entities;

public record Platform : BaseEntity<Guid>
{
    public int ExternalId { get; set; }
    public string Name { get; set; }
    public DateTime? GameReleaseDate { get; set; }
    public string? GameRequirements { get; set; }
}