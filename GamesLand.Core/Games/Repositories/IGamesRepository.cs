using GamesLand.Core.Games.Entities;

namespace GamesLand.Core.Games.Repositories;

public interface IGamesRepository : IRepository<Guid, Game>
{
    Task<Game?> GetByExternalIdAsync(int id);
}