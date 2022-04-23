namespace GamesLand.Infrastructure.RAWG.Entities;

public record RawgEsrbRating
{
    public int Id { get; set; }
    public string Slug { get; set; }
    public string Name { get; set; }
}