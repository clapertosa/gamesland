using GamesLand.Core.Games.Services;
using GamesLand.Core.Users.Services;
using GamesLand.Infrastructure.RAWG.Requests;
using GamesLand.Infrastructure.RAWG.Services;
using GamesLand.Web.Games.Requests;
using Microsoft.AspNetCore.Mvc;

namespace GamesLand.Web.Games.Controllers;

public class GamesController : BaseController
{
    private readonly IGamesService _gamesService;
    private readonly IRawgService _rawgService;
    private readonly IUserAuthentication _userAuthentication;
    private readonly IUsersService _usersService;

    public GamesController(
        IGamesService gamesService,
        IRawgService rawgService,
        IUsersService usersService,
        IUserAuthentication userAuthentication
    )
    {
        _gamesService = gamesService;
        _rawgService = rawgService;
        _usersService = usersService;
        _userAuthentication = userAuthentication;
    }

    [HttpPost("add")]
    public async Task<IActionResult> AddGameToUser(AddGameToUserRequest addGameToUserRequest)
    {
        var userEmail = _userAuthentication.GetUserEmailFromToken(Request.Headers.Authorization[0].Split(" ")[1]);
        var userRecord =
            await _usersService.GetUserByEmailAsync(userEmail);
        await _gamesService.AddGameToUserAsync(
            userRecord.Id,
            addGameToUserRequest.ToGame(),
            addGameToUserRequest.ToPlatform().ExternalId);
        return CreatedAtAction(nameof(AddGameToUser), null);
    }

    [HttpGet("search")]
    public async Task<IActionResult> SearchGame([FromQuery] SearchRequest searchRequest)
    {
        return Ok(await _rawgService.SearchAsync(searchRequest));
    }

    [HttpGet("{gameId:int}")]
    public async Task<IActionResult> GetGame(int gameId)
    {
        return Ok(await _rawgService.GetGame(gameId));
    }
}