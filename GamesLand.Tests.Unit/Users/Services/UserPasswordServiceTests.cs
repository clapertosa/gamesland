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
        var password = "password";
        var hashedPassword = _userAuthentication.Hash(password);

        Assert.True(_userAuthentication.Match(password, hashedPassword));
    }

    [Fact]
    public void Hashed_Password_Not_Matching()
    {
        var password = "password";
        var wrongPassword = "Password";
        var hashedPassword = _userAuthentication.Hash(password);

        Assert.False(_userAuthentication.Match(wrongPassword, hashedPassword));
    }
}