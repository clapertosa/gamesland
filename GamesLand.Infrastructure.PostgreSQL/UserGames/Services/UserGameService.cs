using GamesLand.Core.UserGames.Entities;
using GamesLand.Core.UserGames.Repositories;
using GamesLand.Core.UserGames.Services;

namespace GamesLand.Infrastructure.PostgreSQL.UserGames.Services;

public class UserGameService : IUserGameService
{
    private readonly IUserGameRepository _userGameRepository;

    public UserGameService(IUserGameRepository userGameRepository)
    {
        _userGameRepository = userGameRepository;
    }

    public Task<UserGame> UpdateReleaseDateAsync(Guid gameId, Guid platformId, DateTime releaseDate)
    {
        return _userGameRepository.UpdateReleaseDateAsync(gameId, platformId, releaseDate);
    }
}