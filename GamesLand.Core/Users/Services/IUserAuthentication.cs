using GamesLand.Core.Users.Entities;

namespace GamesLand.Core.Users.Services;

public interface IUserAuthentication
{
    string Hash(string password);
    bool Match(string password, string hashedPassword);
    public string GetToken(User user);
}