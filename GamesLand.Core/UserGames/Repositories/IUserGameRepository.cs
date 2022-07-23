using GamesLand.Core.UserGames.Entities;

namespace GamesLand.Core.UserGames.Repositories;

public interface IUserGameRepository
{
    Task<UserGame> UpdateReleaseDateAsync(Guid gameId, Guid platformId, DateTime releaseDate);
}