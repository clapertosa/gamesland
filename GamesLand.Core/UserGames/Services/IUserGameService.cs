using GamesLand.Core.UserGames.Entities;

namespace GamesLand.Core.UserGames.Services;

public interface IUserGameService
{
    Task<UserGame?> UpdateReleaseDateAsync(Guid gameId, Guid platformId, DateTime? releaseDate);
}