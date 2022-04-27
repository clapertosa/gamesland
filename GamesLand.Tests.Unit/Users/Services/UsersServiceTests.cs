using System;
using System.Net;
using System.Threading.Tasks;
using GamesLand.Core.Errors;
using GamesLand.Core.Users.Entities;
using GamesLand.Core.Users.Repositories;
using GamesLand.Core.Users.Services;
using GamesLand.Infrastructure.PostgreSQL.Users;
using GamesLand.Tests.Unit.Users.Repositories;
using Xunit;

namespace GamesLand.Tests.Unit.Users.Services;

public class UsersServiceTests
{
    private readonly IUsersService _usersService;

    public UsersServiceTests()
    {
        IUsersRepository fakeUsersRepository = new FakeUsersRepository();
        _usersService = new UsersService(fakeUsersRepository, new UserAuthentication(null));
    }

    [Fact]
    public async Task Create_User_If_Email_Not_Exists()
    {
        User user = new User()
        {
            Email = "email@email.com",
            Password = "password"
        };

        User userRecord = await _usersService.SignUpUserAsync(user);

        Assert.NotNull(userRecord);
    }

    [Fact]
    public async Task Not_Create_A_User_If_Email_Exists()
    {
        User user = new User()
        {
            Email = FakeUsersRepository.RegisteredEmail,
            Password = "password"
        };
        RestException exception = await Assert.ThrowsAsync<RestException>(() => _usersService.SignUpUserAsync(user));

        Assert.Equal(HttpStatusCode.Conflict, exception.StatusCode);
    }

    [Fact]
    public async Task Get_User_By_Id_If_Exists()
    {
        User? user = await _usersService.GetUserByIdAsync(FakeUsersRepository.RegisteredId);

        Assert.NotNull(user);
    }

    [Fact]
    public async Task Get_User_By_Id_If_Not_Exists()
    {
        RestException exception =
            await Assert.ThrowsAsync<RestException>(() => _usersService.GetUserByIdAsync(Guid.NewGuid()));

        Assert.Equal(HttpStatusCode.NotFound, exception.StatusCode);
    }

    [Fact]
    public async Task Get_User_By_Email_If_Exists()
    {
        User? user = await _usersService.GetUserByEmailAsync(FakeUsersRepository.RegisteredEmail);

        Assert.NotNull(user);
    }

    [Fact]
    public async Task Get_User_By_Email_If_Not_Exists()
    {
        RestException exception =
            await Assert.ThrowsAsync<RestException>(() => _usersService.GetUserByEmailAsync("random@random.com"));

        Assert.Equal(HttpStatusCode.NotFound, exception.StatusCode);
    }

    [Fact]
    public async Task Update_User_If_Exists()
    {
        User user = new User()
        {
            Email = "email@email.com",
            FirstName = "name",
            LastName = "last_name",
            Password = "password"
        };
        User userRecord = await _usersService.UpdateUserAsync(FakeUsersRepository.RegisteredId, user);

        Assert.Equal(user.Email, userRecord.Email);
        Assert.Equal(user.FirstName, userRecord.FirstName);
        Assert.Equal(user.LastName, userRecord.LastName);
    }

    [Fact]
    public async Task Update_User_If_Not_Exists()
    {
        RestException exception =
            await Assert.ThrowsAsync<RestException>(() => _usersService.UpdateUserAsync(Guid.NewGuid(), new User()));

        Assert.Equal(HttpStatusCode.NotFound, exception.StatusCode);
    }

    [Fact]
    public async Task Delete_User_If_Exists()
    {
        try
        {
            await _usersService.DeleteUserAsync(FakeUsersRepository.RegisteredId);
            Assert.True(true);
        }
        catch (Exception e)
        {
            Assert.True(false);
        }
    }

    [Fact]
    public async Task Delete_User_If_Not_Exists()
    {
        RestException exception =
            await Assert.ThrowsAsync<RestException>(() => _usersService.DeleteUserAsync(Guid.NewGuid()));

        Assert.Equal(HttpStatusCode.NotFound, exception.StatusCode);
    }
}