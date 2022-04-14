using System.Net;
using GamesLand.Core.Errors;
using GamesLand.Core.Users.Repositories;

namespace GamesLand.Core.Users.Services;

public interface IUsersService
{
    Task<User> CreateUserAsync(User user);
}

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
        string hashedPassword = _userPasswordService.Hash(user.Password);
        User? userRecord = await _usersRepository.GetByEmailAsync(user.Email);
        if (userRecord != null)
            throw new RestException(HttpStatusCode.Conflict, new { Meesage = "Email already registered." });
        return await _usersRepository.CreateAsync(user, hashedPassword);
    }
}