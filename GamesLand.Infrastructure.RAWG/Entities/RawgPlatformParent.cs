namespace GamesLand.Infrastructure.RAWG.Entities;

public record RawgPlatformParent
{
    public RawgPlatform Platform { get; set; }
    public DateTime? ReleasedAt { get; set; }
    public RawgRequirement? Requirements { get; set; }
}