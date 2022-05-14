using System.Net.Http.Json;
using GamesLand.Infrastructure.RAWG.Entities;
using GamesLand.Infrastructure.RAWG.Requests;
using Microsoft.Extensions.Configuration;

namespace GamesLand.Infrastructure.RAWG.Services;

public class RawgService : IRawgService
{
    private readonly HttpClient _client;
    private readonly IConfiguration _configuration;

    public RawgService(HttpClient client, IConfiguration configuration)
    {
        _client = client;
        _configuration = configuration;
    }

    private string AddSearchParams(SearchRequest searchRequest)
    {
        string res = "?";
        if (searchRequest.Page > 0) res += $"page={searchRequest.Page}";
        if (searchRequest.PageSize > 0) res += $"page_size={searchRequest.PageSize}";
        if (searchRequest.Platforms.Any()) res += $"platforms={string.Join(",", searchRequest.Platforms)}";
        if (searchRequest.Name.Length > 0) res += $"search={searchRequest.Name}";

        return res.Length > 1 ? res : "";
    }

    public async Task<IEnumerable<RawgGame>> SearchAsync(SearchRequest searchRequest)
    {
        RawgGameResults? res =
            await _client.GetFromJsonAsync<RawgGameResults>("games" + AddSearchParams(searchRequest));
        return res?.Results ?? Array.Empty<RawgGame>();
    }

    public async Task<RawgGame?> GetGame(int gameId)
    {
        return await _client.GetFromJsonAsync<RawgGame>($"games/{gameId}");
    }
}