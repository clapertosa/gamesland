using GamesLand.Core.Users.Services;
using GamesLand.Infrastructure.PostgreSQL.Users;
using Xunit;

namespace GamesLand.Tests.Unit.Users.Services;

public class UserPasswordServiceTests
{
    private readonly IUserPasswordService _userPasswordService;

    public UserPasswordServiceTests()
    {
        _userPasswordService = new UserPasswordService();
    }

    [Fact]
    public void Hashed_Password_Matching()
    {
        string password = "password";
        string hashedPassword = _userPasswordService.Hash(password);

        Assert.True(_userPasswordService.Match(password, hashedPassword));
    }

    [Fact]
    public void Hashed_Password_Not_Matching()
    {
        string password = "password";
        string wrongPassword = "Password";
        string hashedPassword = _userPasswordService.Hash(password);

        Assert.False(_userPasswordService.Match(wrongPassword, hashedPassword));
    }
}