namespace GamesLand.Core.UserGames.Entities;

public class UserGame
{
    public Guid UserId { get; set; }
    public Guid GameId { get; set; }
    public Guid PlatformId { get; set; }
    public bool Notified { get; set; }
    public DateTime? ReleaseDate { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}