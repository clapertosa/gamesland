namespace GamesLand.Infrastructure.RAWG.Entities;

public record RawgRequirement
{
    public string? Minimum { get; set; }
    public string? Recommended { get; set; }
}