using System.Threading.Tasks;
using GamesLand.Core.Errors;
using GamesLand.Core.Users;
using GamesLand.Core.Users.Repositories;
using GamesLand.Core.Users.Services;
using GamesLand.Infrastructure.PostgreSQL.Services;
using GamesLand.Tests.Unit.Users.Repositories;
using Xunit;

namespace GamesLand.Tests.Unit.Users.Services;

public class UsersServiceTests
{
    private readonly IUsersService _usersService;

    public UsersServiceTests()
    {
        IUsersRepository fakeUsersRepository = new FakeUsersRepository();
        _usersService = new UsersService(fakeUsersRepository, new UserPasswordService());
    }

    [Fact]
    public async Task Create_User_If_Email_Not_Exists()
    {
        User user = new User()
        {
            Email = "email@email.com",
            Password = "password"
        };

        User userRecord = await _usersService.CreateUserAsync(user);

        Assert.NotNull(userRecord);
    }

    [Fact]
    public Task Not_Create_A_User_If_Email_Exists()
    {
        User user = new User()
        {
            Email = FakeUsersRepository.RegisteredEmail,
            Password = "password"
        };

        return Assert.ThrowsAsync<RestException>(() =>_usersService.CreateUserAsync(user));
    }
}