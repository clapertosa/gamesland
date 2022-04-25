namespace GamesLand.Infrastructure.RAWG.Entities;

public record RawgGameResults
{
    public int Count { get; set; }
    public string Next { get; set; }
    public string Previous { get; set; }
    public IEnumerable<RawgGame> Results { get; set; }
}