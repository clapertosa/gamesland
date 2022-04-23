using GamesLand.Core.Users.Entities;

namespace GamesLand.Core.Users.Repositories;

public interface IUsersRepository : IRepository<Guid, User>
{
    Task<User?> GetByEmailAsync(string email);
    Task<User> CreateAsync(User entity, string hashedPassword);
}