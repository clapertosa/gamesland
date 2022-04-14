namespace GamesLand.Core.Users.Services;

public interface IUserPasswordService
{
    string Hash(string password);
    bool Match(string password, string hashedPassword);
}