using System.ComponentModel;
using System.Text.Json.Serialization;
using GamesLand.Core.Games.Entities;

namespace GamesLand.Infrastructure.RAWG.Entities;

public record RawgGame
{
    public int Id { get; set; }
    public string Slug { get; set; }
    public string Name { get; set; }
    [JsonPropertyName("name_original")] public string? NameOriginal { get; set; }
    public string? Description { get; set; }
    public int? Metacritic { get; set; }
    public string? Released { get; set; }
    public bool? Tba { get; set; }
    public string? Updated { get; set; }
    [JsonPropertyName("background_image")] public string? BackgroundImage { get; set; }

    [JsonPropertyName("background_image_additional")]
    public string? BackgroundImageAdditional { get; set; }

    public string? Website { get; set; }
    public double? Rating { get; set; }
    [JsonPropertyName("rating_top")] public int? RatingTop { get; set; }
    public int? Added { get; set; }
    [Description("In Hours")] public int? Playtime { get; set; }
    [JsonPropertyName("screenshot_count")] public int? ScreenshotCount { get; set; }
    [JsonPropertyName("movies_count")] public int? MoviesCount { get; set; }
    [JsonPropertyName("creators_count")] public int? CreatorsCount { get; set; }

    [JsonPropertyName("achievements_count")]
    public int? AchievementsCount { get; set; }

    [JsonPropertyName("parent_achievements_count")]
    public int? ParentAchievementsCount { get; set; }

    [JsonPropertyName("reddit_url")] public string? RedditUrl { get; set; }
    [JsonPropertyName("ratings_count")] public int? RatingsCount { get; set; }

    [JsonPropertyName("alternative_names")]
    public string[]? AlternativeNames { get; set; }

    [JsonPropertyName("metacritic_url")] public string? MetacriticUrl { get; set; }
    [JsonPropertyName("parents_count")] public int? ParentsCount { get; set; }
    [JsonPropertyName("additions_count")] public int? AdditionsCount { get; set; }

    [JsonPropertyName("game_series_count")]
    public int? GameSeriesCount { get; set; }

    [JsonPropertyName("esrb_rating")] public RawgEsrbRating? RawgEsrbRating { get; set; }
    public IEnumerable<RawgPlatformParent>? Platforms { get; set; }

    public Game ToGame() => new Game()
    {
        ExternalId = Id,
        Name = Name,
        NameOriginal = NameOriginal,
        Description = Description,
        Released = Released != null ? DateTime.Parse(Released) : null,
        Updated = Updated != null ? DateTime.Parse(Updated) : null,
        ToBeAnnounced = Tba,
        BackgroundImagePath = BackgroundImage,
        BackgroundImageAdditionalPath = BackgroundImageAdditional,
        Website = Website,
        Rating = Rating,
        RatingsCount = RatingsCount,
    };
}