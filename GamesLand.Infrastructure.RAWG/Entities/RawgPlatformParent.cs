using System.Text.Json.Serialization;

namespace GamesLand.Infrastructure.RAWG.Entities;

public record RawgPlatformParent
{
    public RawgPlatform Platform { get; set; }
    [JsonPropertyName("released_at")] public DateTime? ReleasedAt { get; set; }
    public RawgRequirement? Requirements { get; set; }
}