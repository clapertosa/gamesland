using GamesLand.Core.Users.Services;
using GamesLand.Infrastructure.PostgreSQL.Users;
using Xunit;

namespace GamesLand.Tests.Unit.Users.Services;

public class UserPasswordServiceTests
{
    private readonly IUserAuthentication _userAuthentication;

    public UserPasswordServiceTests()
    {
        _userAuthentication = new UserAuthentication(null);
    }

    [Fact]
    public void Hashed_Password_Matching()
    {
        string password = "password";
        string hashedPassword = _userAuthentication.Hash(password);

        Assert.True(_userAuthentication.Match(password, hashedPassword));
    }

    [Fact]
    public void Hashed_Password_Not_Matching()
    {
        string password = "password";
        string wrongPassword = "Password";
        string hashedPassword = _userAuthentication.Hash(password);

        Assert.False(_userAuthentication.Match(wrongPassword, hashedPassword));
    }
}