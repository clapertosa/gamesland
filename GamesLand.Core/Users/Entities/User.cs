using GamesLand.Core.Games.Entities;

namespace GamesLand.Core.Users.Entities;

public record User : BaseEntity<Guid>
{
    public string FirstName { get; set; }
    public string? LastName { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public long TelegramChatId { get; set; }

    public IEnumerable<Game> Games { get; set; }
}