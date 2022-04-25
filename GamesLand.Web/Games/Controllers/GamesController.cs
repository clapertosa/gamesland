using GamesLand.Infrastructure.RAWG.Requests;
using GamesLand.Infrastructure.RAWG.Services;
using GamesLand.Web.Games.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GamesLand.Web.Games.Controllers;

public class GamesController : BaseController
{
    private readonly IRawgService _rawgService;

    public GamesController(IRawgService rawgService)
    {
        _rawgService = rawgService;
    }

    [HttpPost("games")]
    public async Task<IActionResult> AddGameToUser(AddGameToUserRequest addGameToUserRequest)
    {
        throw new NotImplementedException();
    }

    [AllowAnonymous]
    [HttpGet("search")]
    public async Task<IActionResult> SearchGame(SearchRequest searchRequest)
    {
        return Ok(await _rawgService.SearchAsync(searchRequest));
    }

    [AllowAnonymous]
    [HttpGet("{gameId:int}")]
    public async Task<IActionResult> GetGame(int gameId)
    {
        return Ok(await _rawgService.GetGame(gameId));
    }
}