using System.ComponentModel;

namespace GamesLand.Infrastructure.RAWG.Entities;

public record RawgGame
{
    public int Id { get; set; }
    public string Slug { get; set; }
    public string Name { get; set; }
    public string NameOriginal { get; set; }
    public string Description { get; set; }
    public int Metacritic { get; set; }
    public string Released { get; set; }
    public bool ToBeAnnounced { get; set; }
    public string Updated { get; set; }
    public string BackgroundImage { get; set; }
    public string BackgroundImageAdditional { get; set; }
    public string Website { get; set; }
    public double Rating { get; set; }
    public int RatingTop { get; set; }
    public string Added { get; set; }
    [Description("In Hours")] public int Playtime { get; set; }
    public int ScreenshotCount { get; set; }
    public int MoviesCount { get; set; }
    public int CreatorsCount { get; set; }
    public int AchievementsCount { get; set; }
    public string ParentAchievementsCount { get; set; }
    public string RedditUrl { get; set; }
    public int RatingsCount { get; set; }
    public string[] AlternativeNames { get; set; }
    public string MetacriticUrl { get; set; }
    public int ParentsCount { get; set; }
    public int AdditionsCount { get; set; }
    public int GameSeriesCount { get; set; }
    public RawgEsrbRating RawgEsrbRating { get; set; }
}