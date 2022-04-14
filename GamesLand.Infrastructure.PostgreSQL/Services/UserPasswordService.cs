using GamesLand.Core.Users.Services;

namespace GamesLand.Infrastructure.PostgreSQL.Services;

public class UserPasswordService : IUserPasswordService
{
    public string Hash(string password) => BCrypt.Net.BCrypt.HashPassword(password);

    public bool Match(string password, string hashedPassword) => BCrypt.Net.BCrypt.Verify(password, hashedPassword);
}