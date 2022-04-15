using System;
using System.Threading.Tasks;
using GamesLand.Core.Users;
using GamesLand.Core.Users.Repositories;
using GamesLand.Infrastructure.PostgreSQL.Users;
using GamesLand.Tests.Helpers.PostgreSQL;
using Xunit;

namespace GamesLand.Tests.Integration.PostgreSQL.Users;

public class UsersRepositoryTests : PostgreSqlHelper, IDisposable
{
    private readonly IUsersRepository _usersRepository;

    public UsersRepositoryTests()
    {
        _usersRepository = new UsersRepository(Connection);
    }

    [Fact]
    public async Task Create_User_Successfully()
    {
        User user = new User()
        {
            Email = "email@email.com",
            Password = "password"
        };
        var userRecord = await _usersRepository.CreateAsync(user);

        Assert.Equal(user.Email, userRecord.Email);
    }

    [Fact]
    public async Task Get_User_By_Email_With_Existing_User()
    {
        User user = new User() { Email = "email@email.com", Password = "password" };
        await _usersRepository.CreateAsync(user);
        User? userRecord = await _usersRepository.GetByEmailAsync(user.Email);

        Assert.NotNull(userRecord);
    }

    [Fact]
    public async Task Get_User_By_Email_With_Non_Existing_user()
    {
        User? userRecord = await _usersRepository.GetByEmailAsync("email@email.com");

        Assert.Null(userRecord);
    }

    public new async void Dispose()
    {
        await DropTables();
    }
}