using GamesLand.Infrastructure.RAWG.Entities;
using GamesLand.Infrastructure.RAWG.Requests;

namespace GamesLand.Infrastructure.RAWG.Services;

public interface IRawgService
{
    Task<IEnumerable<RawgGame>> SearchAsync(SearchRequest searchRequest);
    Task<RawgGame?> GetGame(int gameId);
}