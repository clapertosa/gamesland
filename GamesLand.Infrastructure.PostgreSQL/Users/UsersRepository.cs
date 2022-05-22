using System.Data;
using Dapper;
using GamesLand.Core.Users.Entities;
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
            "INSERT INTO users(first_name, last_name, email, telegram_chat_id, password) VALUES(@FirstName, @LastName, @Email, @TelegramChatId, @Password) RETURNING *";
        var userPersistent = await _connection.QueryFirstAsync<UserPersistent>(query,
            new { entity.FirstName, entity.LastName, entity.Email, entity.TelegramChatId, entity.Password });
        return userPersistent.ToUser();
    }

    public async Task<User> CreateAsync(User entity, string hashedPassword)
    {
        const string query =
            "INSERT INTO users(first_name, last_name, email, telegram_chat_id, password) VALUES(@FirstName, @LastName, @Email, @TelegramChatId, @Password) RETURNING id, first_name, last_name, email, telegram_chat_id, created_at, updated_at";
        var userPersistent = await _connection.QueryFirstAsync<UserPersistent>(query,
            new { entity.FirstName, entity.LastName, entity.Email, entity.TelegramChatId, Password = hashedPassword });
        return userPersistent.ToUser();
    }

    public async Task<IEnumerable<User>> GetAllAsync(int page, int pageSize)
    {
        var query =
            $"SELECT id, first_name, last_name, email, telegram_chat_id, created_at, updated_at FROM users ORDER BY email ASC LIMIT {pageSize} OFFSET {page * pageSize}";
        var usersPersistent = await _connection.QueryAsync<UserPersistent>(query);
        return usersPersistent.Select(x => x.ToUser());
    }

    public async Task<User?> GetByIdAsync(Guid id)
    {
        const string query =
            "SELECT id, first_name, last_name, email, telegram_chat_id, created_at, updated_at FROM users WHERE id = @Id";
        var userPersistent =
            await _connection.QueryFirstOrDefaultAsync<UserPersistent>(query, new { Id = id });
        return userPersistent?.ToUser();
    }

    public async Task<User?> GetByEmailAsync(string email)
    {
        const string query =
            "SELECT id, first_name, last_name, email, telegram_chat_id, password, created_at, updated_at FROM users WHERE email = @Email";
        var user = await _connection.QueryFirstOrDefaultAsync<UserPersistent>(query, new { Email = email });
        return user?.ToUser();
    }

    public async Task<User> UpdateAsync(Guid id, User entity)
    {
        const string query =
            "UPDATE users SET first_name = @FirstName, last_name = @LastName, updated_at = current_timestamp WHERE id = @Id RETURNING id, first_name, last_name, email, telegram_chat_id, created_at, updated_at";
        var user = await _connection.QueryFirstOrDefaultAsync<UserPersistent>(query,
            new { Id = id, entity.FirstName, entity.LastName, UpdatedAt = DateTime.UtcNow });
        return user.ToUser();
    }

    public Task DeleteAsync(Guid id)
    {
        const string query = "DELETE FROM users WHERE id = @Id";
        return _connection.ExecuteAsync(query, new { Id = id });
    }
}