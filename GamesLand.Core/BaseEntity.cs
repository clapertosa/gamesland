namespace GamesLand.Core;

public abstract record BaseEntity<TIdentity>
{
    public TIdentity Id { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}