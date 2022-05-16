using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using GamesLand.Core.Errors;
using GamesLand.Core.Games.Entities;
using GamesLand.Core.Games.Repositories;

namespace GamesLand.Tests.Unit.Games.Repositories;

public class FakeGamesRepository : IGamesRepository
{
    public static Guid RegisteredId => Guid.Parse("85c6aa9b-d2d6-4eb3-a44c-453eff606b13");
    public static int RegisteredExternalId => 1234;

    private Game GetGame(Game entity)
    {
        return new Game()
        {
            Id = entity.Id,
            ExternalId = entity.ExternalId,
            Name = entity.Name,
            NameOriginal = entity.NameOriginal,
            Description = entity.Description,
            Rating = entity.Rating,
            Released = entity.Released,
            Updated = entity.Updated,
            Website = entity.Website,
            RatingsCount = entity.RatingsCount,
            BackgroundImagePath = entity.BackgroundImagePath,
            BackgroundImageAdditionalPath = entity.BackgroundImageAdditionalPath,
            ToBeAnnounced = entity.ToBeAnnounced,
            CreatedAt = entity.CreatedAt,
            UpdatedAt = entity.UpdatedAt
        };
    }

    public Task<Game> CreateAsync(Game entity)
    {
        return Task.FromResult(GetGame(entity));
    }

    public Task<Game?> GetByIdAsync(Guid id)
    {
        return id == RegisteredId
            ? Task.FromResult<Game?>(GetGame(new Game() { Id = RegisteredId }))
            : Task.FromResult<Game?>(null);
    }

    public async Task<IEnumerable<Game>> GetAllAsync(int page = 0, int pageSize = 10)
    {
        IEnumerable<Task<Game>> games = new List<Task<Game>>();
        for (int i = 0; i < pageSize; i++)
        {
            games.Append(Task.FromResult(GetGame(new Game())));
        }

        return await Task.WhenAll(games);
    }

    public Task<Game> UpdateAsync(Guid id, Game entity)
    {
        return id == RegisteredId ? Task.FromResult(GetGame(entity)) : Task.FromResult<Game>(null);
    }

    public Task DeleteAsync(Guid id)
    {
        return id == RegisteredId
            ? Task.CompletedTask
            : Task.FromException(new RestException(HttpStatusCode.NotFound, new { Message = "Game not found." }));
    }

    public Task<Game?> GetByExternalIdAsync(int externalId)
    {
        return externalId == RegisteredExternalId
            ? Task.FromResult<Game?>(GetGame(new Game() { ExternalId = externalId }))
            : Task.FromResult<Game?>(null);
    }

    public Task SaveUserGameReleaseDateAsync(Guid userId, Guid gameId, Guid platformId, DateTime? releaseDate)
    {
        return Task.CompletedTask;
    }

    public Task RemoveGameFromUserAsync(Guid userId, Guid gameId, int platformId)
    {
        return Task.CompletedTask;
    }

    public async Task<IEnumerable<Game>> GetUsersGameGroupedByGameIdAsync()
    {
        return await Task.FromResult(new[] { new Game() });
    }

    public async Task<IEnumerable<Game>> GetReleasedUsersGameGroupedByUserIdAsync()
    {
        return await Task.FromResult(new[] { new Game() });
    }
}