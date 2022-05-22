using System.Net;
using GamesLand.Core.Errors;
using GamesLand.Core.Users.Entities;
using GamesLand.Core.Users.Repositories;

namespace GamesLand.Core.Users.Services;

public class UsersService : IUsersService
{
    private readonly IUserAuthentication _userAuthentication;
    private readonly IUsersRepository _usersRepository;

    public UsersService(IUsersRepository usersRepository, IUserAuthentication userAuthentication)
    {
        _usersRepository = usersRepository;
        _userAuthentication = userAuthentication;
    }

    public async Task<User> SignUpUserAsync(User user)
    {
        var userRecord = await _usersRepository.GetByEmailAsync(user.Email);
        if (userRecord != null)
            throw new RestException(HttpStatusCode.Conflict, new { Meesage = "Email already registered." });
        var hashedPassword = _userAuthentication.Hash(user.Password);
        return await _usersRepository.CreateAsync(user, hashedPassword);
    }

    public async Task<string> SignInUserAsync(User user)
    {
        var userRecord = await _usersRepository.GetByEmailAsync(user.Email);

        if (userRecord == null || !_userAuthentication.Match(user.Password, userRecord.Password))
            throw new RestException(HttpStatusCode.Forbidden, new { Message = "Email or Password wrong." });

        return _userAuthentication.GetToken(user);
    }

    public async Task<User?> GetUserByIdAsync(Guid id)
    {
        var userRecord = await _usersRepository.GetByIdAsync(id);
        if (userRecord == null) throw new RestException(HttpStatusCode.NotFound, new { Message = "User not found." });
        return userRecord;
    }

    public async Task<User?> GetUserByEmailAsync(string email)
    {
        var userRecord = await _usersRepository.GetByEmailAsync(email);
        if (userRecord == null) throw new RestException(HttpStatusCode.NotFound, new { Message = "User not found." });
        return userRecord;
    }

    public Task<IEnumerable<User>> GetUsersAsync(int page = 10, int pageSize = 0)
    {
        return _usersRepository.GetAllAsync(page, pageSize);
    }

    public async Task<User> UpdateUserAsync(Guid id, User entity)
    {
        var userRecord = await _usersRepository.GetByIdAsync(id);
        if (userRecord == null) throw new RestException(HttpStatusCode.NotFound, new { Message = "User not found." });
        return await _usersRepository.UpdateAsync(id, entity);
    }

    public async Task DeleteUserAsync(Guid id)
    {
        var userRecord = await _usersRepository.GetByIdAsync(id);
        if (userRecord == null) throw new RestException(HttpStatusCode.NotFound, new { Message = "User not found." });
        await _usersRepository.DeleteAsync(id);
    }
}