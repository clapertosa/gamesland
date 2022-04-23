using GamesLand.Core.Users.Entities;

namespace GamesLand.Core.Users.Services;

public interface IUsersService
{
    Task<User> CreateUserAsync(User user);
    Task<User?> GetUserByIdAsync(Guid id);
    Task<User?> GetUserByEmailAsync(string email);
    Task<IEnumerable<User>> GetUsersAsync(int page = 10, int pageSize = 1);
    Task<User> UpdateUserAsync(Guid id, User entity);
    Task DeleteUserAsync(Guid id);
}