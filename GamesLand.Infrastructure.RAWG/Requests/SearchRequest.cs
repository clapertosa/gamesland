namespace GamesLand.Infrastructure.RAWG.Requests;

public record SearchRequest
{
    public string Name { get; set; }
    public IEnumerable<int> Platforms { get; set; } = new List<int>();
    public int Page { get; set; }
    public int PageSize { get; set; }
}