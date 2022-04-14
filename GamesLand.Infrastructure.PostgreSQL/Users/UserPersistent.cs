using GamesLand.Core;
using GamesLand.Core.Users;

namespace GamesLand.Infrastructure.PostgreSQL.Users;

internal record UserPersistent : BaseEntity<Guid>
{
    public string FirstName { get; set; }
    public string? LastName { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }

    public User ToUser()
    {
        return new User()
        {
            Id = Id,
            FirstName = FirstName,
            LastName = LastName,
            Email = Email,
            Password = Password,
            CreatedAt = CreatedAt,
            UpdatedAt = UpdatedAt
        };
    }
}