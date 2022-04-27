using GamesLand.Core.Users.Entities;
using GamesLand.Core.Users.Services;
using GamesLand.Web.Users.Requests;
using GamesLand.Web.Users.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GamesLand.Web.Users.Controllers;

public class UsersController : BaseController
{
    private readonly IUsersService _usersService;

    public UsersController(IUsersService usersService)
    {
        _usersService = usersService;
    }

    [AllowAnonymous]
    [HttpPost("signup")]
    public async Task<IActionResult> SignUp(SignUpUserRequest user)
    {
        User newUser = await _usersService.SignUpUserAsync(user.ToUser());
        return CreatedAtAction(nameof(SignUp), UserResponse.FromUser(newUser));
    }

    [AllowAnonymous]
    [HttpPost("signin")]
    public async Task<IActionResult> SignIn(SignInUserRequest user)
    {
        return Ok(await _usersService.SignInUserAsync(user.ToUser()));
    }

    [HttpGet("{Guid id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        User? user = await _usersService.GetUserByIdAsync(id);
        return Ok(UserResponse.FromUser(user));
    }
}