using System.Net;
using GamesLand.Core.Errors;
using GamesLand.Core.Users.Entities;
using GamesLand.Core.Users.Repositories;

namespace GamesLand.Core.Users.Services;

public class UsersService : IUsersService
{
    private readonly IUsersRepository _usersRepository;
    private readonly IUserPasswordService _userPasswordService;

    public UsersService(IUsersRepository usersRepository, IUserPasswordService userPasswordService)
    {
        _usersRepository = usersRepository;
        _userPasswordService = userPasswordService;
    }

    public async Task<User> CreateUserAsync(User user)
    {
        User? userRecord = await _usersRepository.GetByEmailAsync(user.Email);
        if (userRecord != null)
            throw new RestException(HttpStatusCode.Conflict, new { Meesage = "Email already registered." });
        string hashedPassword = _userPasswordService.Hash(user.Password);
        return await _usersRepository.CreateAsync(user, hashedPassword);
    }

    public async Task<User?> GetUserByIdAsync(Guid id)
    {
        User? userRecord = await _usersRepository.GetByIdAsync(id);
        if (userRecord == null) throw new RestException(HttpStatusCode.NotFound, new { Message = "User not found." });
        return userRecord;
    }

    public async Task<User?> GetUserByEmailAsync(string email)
    {
        User? userRecord = await _usersRepository.GetByEmailAsync(email);
        if (userRecord == null) throw new RestException(HttpStatusCode.NotFound, new { Message = "User not found." });
        return userRecord;
    }

    public Task<IEnumerable<User>> GetUsersAsync(int page = 10, int pageSize = 0)
    {
        return _usersRepository.GetAllAsync(page, pageSize);
    }

    public async Task<User> UpdateUserAsync(Guid id, User entity)
    {
        User? userRecord = await _usersRepository.GetByIdAsync(id);
        if (userRecord == null) throw new RestException(HttpStatusCode.NotFound, new { Message = "User not found." });
        return await _usersRepository.UpdateAsync(id, entity);
    }

    public async Task DeleteUserAsync(Guid id)
    {
        User? userRecord = await _usersRepository.GetByIdAsync(id);
        if (userRecord == null) throw new RestException(HttpStatusCode.NotFound, new { Message = "User not found." });
        await _usersRepository.DeleteAsync(id);
    }
}