using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GamesLand.Core.Users;
using GamesLand.Core.Users.Repositories;

namespace GamesLand.Tests.Unit.Users.Repositories;

public class FakeUsersRepository : IUsersRepository
{
    public static string RegisteredEmail => "registered@registered.com";

    private User? GetUser(User entity)
    {
        return new User()
        {
            Id = Guid.NewGuid(),
            Email = entity.Email,
            Password = entity.Password,
            FirstName = entity.FirstName,
            LastName = entity.LastName,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
    }

    public Task<User> CreateAsync(User entity)
    {
        return Task.FromResult(GetUser(entity))!;
    }

    public async Task<User> GetByIdAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<User>> GetAllAsync()
    {
        throw new NotImplementedException();
    }

    public async Task<User> UpdateAsync(Guid id, User entity)
    {
        throw new NotImplementedException();
    }

    public async Task DeleteAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task<User?> GetByEmailAsync(string email)
    {
        return email == RegisteredEmail ? Task.FromResult(GetUser(new User())) : Task.FromResult((User?)null);
    }

    public Task<User> CreateAsync(User entity, string hashedPassword)
    {
        return Task.FromResult(GetUser(entity));
    }
}