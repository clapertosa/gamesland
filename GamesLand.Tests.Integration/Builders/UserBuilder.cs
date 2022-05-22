using System.Collections.Generic;
using GamesLand.Core.Games.Entities;
using GamesLand.Core.Users.Entities;

namespace GamesLand.Tests.Integration.Builders;

public class UserBuilder
{
    private readonly User _user = new();

    public UserBuilder WithFirstName(string firstName)
    {
        _user.FirstName = firstName;
        return this;
    }

    public UserBuilder WithLastName(string lastName)
    {
        _user.LastName = lastName;
        return this;
    }

    public UserBuilder WithEmail(string email)
    {
        _user.Email = email;
        return this;
    }

    public UserBuilder WithPassword(string password)
    {
        _user.Password = password;
        return this;
    }

    public UserBuilder WithGames(IEnumerable<Game> games)
    {
        _user.Games = games;
        return this;
    }

    public User Build()
    {
        return _user;
    }
}