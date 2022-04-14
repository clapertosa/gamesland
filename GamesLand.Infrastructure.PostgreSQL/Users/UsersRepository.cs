using System.Data;
using Dapper;
using GamesLand.Core.Users;
using GamesLand.Core.Users.Repositories;

namespace GamesLand.Infrastructure.PostgreSQL.Users;

public class UsersRepository : IUsersRepository
{
    private readonly IDbConnection _connection;

    public UsersRepository(IDbConnection connection)
    {
        _connection = connection;
    }

    public async Task<User> CreateAsync(User entity)
    {
        const string query =
            "INSERT INTO users(first_name, last_name, email, password) VALUES(@FirstName, @LastName, @Email, @Password) RETURNING *";
        UserPersistent userPersistent = await _connection.QueryFirstAsync<UserPersistent>(query,
            new { entity.FirstName, entity.LastName, entity.Email, entity.Password });
        return userPersistent.ToUser();
    }

    public async Task<User> CreateAsync(User entity, string hashedPassword)
    {
        const string query =
            "INSERT INTO users(first_name, last_name, email, password) VALUES(@FirstName, @LastName, @Email, @Password) RETURNING *";
        UserPersistent userPersistent = await _connection.QueryFirstAsync<UserPersistent>(query,
            new { entity.FirstName, entity.LastName, entity.Email, Password = hashedPassword });
        return userPersistent.ToUser();
    }

    public async Task<IEnumerable<User>> GetAllAsync()
    {
        throw new NotImplementedException();
    }

    public async Task<User> GetByIdAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public async Task<User?> GetByEmailAsync(string email)
    {
        const string query = "SELECT * FROM users WHERE email = @Email";
        UserPersistent user = await _connection.QueryFirstOrDefaultAsync<UserPersistent>(query, new { Email = email });
        return user?.ToUser();
    }

    public async Task<User> UpdateAsync(Guid id, User entity)
    {
        throw new NotImplementedException();
    }

    public async Task DeleteAsync(Guid id)
    {
        throw new NotImplementedException();
    }
}