using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using GamesLand.Core.Errors;
using GamesLand.Core.Users.Entities;
using GamesLand.Core.Users.Repositories;

namespace GamesLand.Tests.Unit.Users.Repositories;

public class FakeUsersRepository : IUsersRepository
{
    public static string RegisteredEmail => "registered@registered.com";
    public static Guid RegisteredId => Guid.Parse("a2ffb9ab-81ef-4713-a572-1185487d0b3d");

    private User GetUser(User entity)
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

    public Task<User?> GetByIdAsync(Guid id)
    {
        return id == RegisteredId
            ? Task.FromResult<User?>(GetUser(new User() { Id = RegisteredId }))
            : Task.FromResult<User?>(null);
    }

    public async Task<IEnumerable<User>> GetAllAsync(int page, int pageSize)
    {
        IEnumerable<Task<User>> users = new List<Task<User>>();
        for (int i = 0; i < pageSize; i++)
        {
            users.Append(Task.FromResult(GetUser(new User())));
        }

        return await Task.WhenAll(users);
    }

    public Task<User> UpdateAsync(Guid id, User entity)
    {
        return id == RegisteredId ? Task.FromResult(GetUser(entity)) : Task.FromResult<User>(null);
    }

    public Task DeleteAsync(Guid id)
    {
        return id == RegisteredId
            ? Task.CompletedTask
            : Task.FromException(new RestException(HttpStatusCode.NotFound, new { Message = "User not found." }));
    }

    public Task<User?> GetByEmailAsync(string email)
    {
        return email == RegisteredEmail ? Task.FromResult<User?>(GetUser(new User() {Email = RegisteredEmail})) : Task.FromResult((User?)null);
    }

    public Task<User> CreateAsync(User entity, string hashedPassword)
    {
        return Task.FromResult(GetUser(new User()
        {
            Id = Guid.NewGuid(),
            Email = entity.Email,
            Password = hashedPassword,
            FirstName = entity.FirstName,
            LastName = entity.LastName,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        }));
    }
}