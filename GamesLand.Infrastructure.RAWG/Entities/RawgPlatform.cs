namespace GamesLand.Infrastructure.RAWG.Entities;

public record RawgPlatform
{
    public int Id { get; set; }
    public string Slug { get; set; }

    public string Name { get; set; }

    // YYYY-MM-DD
    public string? ReleasedAt { get; set; }
    public IEnumerable<RawgRequirement>? Requirements { get; set; }
}