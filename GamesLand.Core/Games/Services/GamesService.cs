using System.Net;
using GamesLand.Core.Errors;
using GamesLand.Core.Games.Entities;
using GamesLand.Core.Games.Repositories;

namespace GamesLand.Core.Games.Services;

public class GamesService : IGamesService
{
    private readonly IGamesRepository _gamesRepository;

    public GamesService(IGamesRepository gamesRepository)
    {
        _gamesRepository = gamesRepository;
    }

    public async Task<Game> CreateGameAsync(Game game)
    {
        Game? gameRecord = await _gamesRepository.GetByExternalIdAsync(game.ExternalId);
        if (gameRecord != null)
            throw new RestException(HttpStatusCode.Conflict, new { Message = "Game already exists." });
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
}