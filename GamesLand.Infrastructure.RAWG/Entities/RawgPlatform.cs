using System.Text.Json.Serialization;

namespace GamesLand.Infrastructure.RAWG.Entities;

public record RawgPlatform
{
    public int Id { get; set; }
    public string Slug { get; set; }
    public string Name { get; set; }
    public string? ReleasedAt { get; set; }
    public IEnumerable<RawgRequirement>? Requirements { get; set; }
};