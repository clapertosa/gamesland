﻿using GamesLand.Core.Users;
using GamesLand.Core.Users.Services;
using GamesLand.Web.Users.Requests;
using GamesLand.Web.Users.Responses;
using Microsoft.AspNetCore.Mvc;

namespace GamesLand.Web.Users.Controllers;

public class UsersController : BaseController
{
    private readonly IUsersService _usersService;

    public UsersController(IUsersService usersService)
    {
        _usersService = usersService;
    }

    [HttpPost]
    public async Task<IActionResult> SignUp(CreateUserRequest user)
    {
        User newUser = await _usersService.CreateUserAsync(user.ToUser());
        return Ok(UserResponse.FromUser(newUser));
    }
}