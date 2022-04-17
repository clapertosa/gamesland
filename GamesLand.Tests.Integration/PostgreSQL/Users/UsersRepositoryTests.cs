using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GamesLand.Core.Users;
using GamesLand.Core.Users.Repositories;
using GamesLand.Infrastructure.PostgreSQL.Users;
using GamesLand.Tests.Helpers;
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
        User user = new User()
        {
            FirstName = "First name",
            LastName = "Last name",
            Email = "email@email.com",
            Password = "password"
        };
        var userRecord = await _usersRepository.CreateAsync(user);

        Assert.Equal(user.FirstName, userRecord.FirstName);
        Assert.Equal(user.LastName, userRecord.LastName);
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

    [Fact]
    public async Task Get_User_By_Id_When_Exists()
    {
        User user = new User() { Email = "email@email.com", Password = "password" };
        User registeredUser = await _usersRepository.CreateAsync(user);
        User? userRecord = await _usersRepository.GetByIdAsync(registeredUser.Id);

        Assert.NotNull(userRecord);
    }

    [Fact]
    public async Task Get_User_By_Id_When_Not_Exists()
    {
        User? userRecord = await _usersRepository.GetByIdAsync(Guid.NewGuid());

        Assert.Null(userRecord);
    }

    [Fact]
    public async Task Get_All_Users()
    {
        int pageSize = 5;
        int page = 0;
        int counter = 0;
        for (int i = 0; i < 10; i++)
        {
            string email = $"email@email{counter}.com";
            await _usersRepository.CreateAsync(new User() { Email = email, Password = "password" });
            counter++;
        }

        IEnumerable<User> users = await _usersRepository.GetAllAsync(page, pageSize);

        Assert.Equal(pageSize, users.Count());
    }

    [Fact]
    public async Task Update_User()
    {
        User user = await _usersRepository.CreateAsync(new User() { Email = "email@email.com", Password = "password" });
        user.FirstName = "Name";
        user.LastName = "Last name";
        await _usersRepository.UpdateAsync(user.Id, user);
        User? userRecord = await _usersRepository.GetByIdAsync(user.Id);

        Assert.Equal(user.FirstName, userRecord?.FirstName);
        Assert.Equal(user.LastName, userRecord?.LastName);
    }

    [Fact]
    public async Task Delete_User()
    {
        User user = await _usersRepository.CreateAsync(new User() { Email = "email@email.com", Password = "password" });
        await _usersRepository.DeleteAsync(user.Id);
        IEnumerable<User> users = await _usersRepository.GetAllAsync();

        Assert.Empty(users);
    }
}