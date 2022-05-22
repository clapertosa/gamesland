using System;
using System.Linq;
using System.Threading.Tasks;
using GamesLand.Core.Users.Entities;
using GamesLand.Core.Users.Repositories;
using GamesLand.Infrastructure.PostgreSQL.Users;
using GamesLand.Tests.Helpers;
using GamesLand.Tests.Integration.Builders;
using Xunit;

namespace GamesLand.Tests.Integration.PostgreSQL.Users;

public class UsersRepositoryTests : IntegrationTestBase
{
    private readonly IUsersRepository _usersRepository;

    public UsersRepositoryTests()
    {
        _usersRepository = new UsersRepository(Connection);
    }

    [Fact]
    public async Task Create_User_Successfully()
    {
        var user = new UserBuilder()
            .WithFirstName("First name")
            .WithLastName("Last Name")
            .WithEmail("email@email.com")
            .WithPassword("password")
            .Build();
        var userRecord = await _usersRepository.CreateAsync(user);

        Assert.Equal(user.FirstName, userRecord.FirstName);
        Assert.Equal(user.LastName, userRecord.LastName);
        Assert.Equal(user.Email, userRecord.Email);
    }

    [Fact]
    public async Task Get_User_By_Email_With_Existing_User()
    {
        var user = new UserBuilder()
            .WithEmail("email@email.com")
            .WithPassword("password")
            .Build();
        await _usersRepository.CreateAsync(user);
        var userRecord = await _usersRepository.GetByEmailAsync(user.Email);

        Assert.NotNull(userRecord);
    }

    [Fact]
    public async Task Get_User_By_Email_With_Non_Existing_user()
    {
        var userRecord = await _usersRepository.GetByEmailAsync("email@email.com");

        Assert.Null(userRecord);
    }

    [Fact]
    public async Task Get_User_By_Id_When_Exists()
    {
        var user = new UserBuilder()
            .WithEmail("email@email.com")
            .WithPassword("password")
            .Build();
        var registeredUser = await _usersRepository.CreateAsync(user);
        var userRecord = await _usersRepository.GetByIdAsync(registeredUser.Id);

        Assert.NotNull(userRecord);
    }

    [Fact]
    public async Task Get_User_By_Id_When_Not_Exists()
    {
        var userRecord = await _usersRepository.GetByIdAsync(Guid.NewGuid());

        Assert.Null(userRecord);
    }

    [Fact]
    public async Task Get_All_Users()
    {
        var pageSize = 5;
        var page = 0;
        var counter = 0;
        for (var i = 0; i < 10; i++)
        {
            var email = $"email@email{counter}.com";
            await _usersRepository.CreateAsync(new User { Email = email, Password = "password" });
            counter++;
        }

        var users = await _usersRepository.GetAllAsync(page, pageSize);

        Assert.Equal(pageSize, users.Count());
    }

    [Fact]
    public async Task Update_User()
    {
        var user = await _usersRepository.CreateAsync(new User { Email = "email@email.com", Password = "password" });
        user.FirstName = "Name";
        user.LastName = "Last name";
        await _usersRepository.UpdateAsync(user.Id, user);
        var userRecord = await _usersRepository.GetByIdAsync(user.Id);

        Assert.Equal(user.FirstName, userRecord?.FirstName);
        Assert.Equal(user.LastName, userRecord?.LastName);
    }

    [Fact]
    public async Task Delete_User()
    {
        var user = await _usersRepository.CreateAsync(new User { Email = "email@email.com", Password = "password" });
        await _usersRepository.DeleteAsync(user.Id);
        var users = await _usersRepository.GetAllAsync();

        Assert.Empty(users);
    }
}