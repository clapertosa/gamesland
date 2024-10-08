﻿using System.Data;
using Dapper;
using GamesLand.Core.Platforms.Entities;
using GamesLand.Core.Platforms.Repositories;

namespace GamesLand.Infrastructure.PostgreSQL.Platforms;

public class PlatformsRepository : IPlatformsRepository
{
    private readonly IDbConnection _connection;

    public PlatformsRepository(IDbConnection connection)
    {
        _connection = connection;
    }

    public async Task<Platform> CreateAsync(Platform entity)
    {
        const string query =
            "INSERT INTO platforms(external_id, name) VALUES(@ExternalId, @Name) ON CONFLICT(external_id) DO UPDATE SET name = @Name RETURNING *";
        var platformPersistent =
            await _connection.QueryFirstAsync<PlatformPersistent>(query, new { entity.ExternalId, entity.Name });
        return platformPersistent.ToPlatform();
    }

    public async Task<Platform?> GetByIdAsync(Guid id)
    {
        const string query = "SELECT * FROM platforms WHERE id = @Id";
        var platformPersistent =
            await _connection.QueryFirstOrDefaultAsync<PlatformPersistent>(query, new { Id = id });
        return platformPersistent?.ToPlatform();
    }

    public async Task<IEnumerable<Platform>> GetAllAsync(int page = 0, int pageSize = 10)
    {
        throw new NotImplementedException();
    }

    public async Task<Platform> UpdateAsync(Guid id, Platform entity)
    {
        const string query =
            "UPDATE platforms SET name = @Name, external_id = @ExternalId, updated_at = current_timestamp WHERE id = @Id RETURNING *";
        var platformPersistent =
            await _connection.QueryFirstAsync<PlatformPersistent>(query,
                new { entity.Name, entity.ExternalId, Id = id });
        return platformPersistent.ToPlatform();
    }

    public async Task DeleteAsync(Guid id)
    {
        const string query = "DELETE FROM platforms WHERE id = @Id";
        await _connection.ExecuteAsync(query, new { Id = id });
    }

    public async Task<IEnumerable<Platform>> CreateMultipleAsync(IEnumerable<Platform> platforms)
    {
        var platformsPersistent = new List<PlatformPersistent>();
        const string query =
            "INSERT INTO platforms(external_id, name) VALUES(@ExternalId, @Name) ON CONFLICT(external_id) DO UPDATE SET name = @Name, updated_at = current_timestamp RETURNING *";
        foreach (var p in platforms)
            platformsPersistent.Add(await _connection.QueryFirstAsync<PlatformPersistent>(query,
                new { p.ExternalId, p.Name }));

        return platformsPersistent.Select(x => x.ToPlatform());
    }

    public async Task<Platform?> GetByExternalIdAsync(int externalId)
    {
        const string query = "SELECT * FROM platforms WHERE external_id = @ExternalId";
        var platformPersistent =
            await _connection.QueryFirstOrDefaultAsync<PlatformPersistent>(query, new { ExternalId = externalId });
        return platformPersistent?.ToPlatform();
    }
}