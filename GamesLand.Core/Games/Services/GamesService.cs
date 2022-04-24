using System.Net;
using GamesLand.Core.Errors;
using GamesLand.Core.Games.Entities;
using GamesLand.Core.Games.Repositories;
using GamesLand.Core.Platforms.Services;
using GamesLand.Core.Users.Entities;
using GamesLand.Core.Users.Services;

namespace GamesLand.Core.Games.Services;

public class GamesService : IGamesService
{
    private readonly IGamesRepository _gamesRepository;
    private readonly IPlatformsService _platformsService;
    private readonly IUsersService _usersService;

    public GamesService(
        IGamesRepository gamesRepository,
        IPlatformsService platformsService,
        IUsersService usersService
    )
    {
        _gamesRepository = gamesRepository;
        _platformsService = platformsService;
        _usersService = usersService;
    }

    public async Task<Game> CreateGameAsync(Game game)
    {
        return await _gamesRepository.CreateAsync(game);
    }

    public async Task<Game?> GetGameByIdAsync(Guid id)
    {
        Game? gameRecord = await _gamesRepository.GetByIdAsync(id);
        if (gameRecord == null)
            throw new RestException(HttpStatusCode.NotFound, new { Message = "Game not found." });
        return gameRecord;
    }

    public async Task<Game?> GetGameByExternalIdAsync(int id)
    {
        Game? gameRecord = await _gamesRepository.GetByExternalIdAsync(id);
        if (gameRecord == null)
            throw new RestException(HttpStatusCode.NotFound, new { Message = "Game not found." });
        return gameRecord;
    }

    public async Task<Game> UpdateGameAsync(Guid id, Game game)
    {
        Game? gameRecord = await _gamesRepository.GetByIdAsync(id);
        if (gameRecord == null) throw new RestException(HttpStatusCode.NotFound, new { Message = "Game not found." });
        return await _gamesRepository.UpdateAsync(id, game);
    }

    public async Task DeleteGameAsync(Guid id)
    {
        Game? gameRecord = await _gamesRepository.GetByIdAsync(id);
        if (gameRecord == null) throw new RestException(HttpStatusCode.NotFound, new { Message = "Game not found." });
        await _gamesRepository.DeleteAsync(id);
    }

    public async Task AddGameToUserAsync(Guid userId, Game game, int platformId)
    {
        User? userRecord = await _usersService.GetUserByIdAsync(userId);
        if (userRecord == null) throw new RestException(HttpStatusCode.NotFound, new { Message = "User not found." });

        Game? gameRecord = await _gamesRepository.GetByExternalIdAsync(game.ExternalId);
        if (gameRecord != null) await _gamesRepository.UpdateAsync(gameRecord.Id, game);
        else gameRecord = await _gamesRepository.CreateAsync(game);

        await _platformsService.CreateMultiplePlatformsAsync(game.Platforms);

        await _gamesRepository.AddGameToUserAsync(userRecord.Id, gameRecord.Id, platformId);
    }
}