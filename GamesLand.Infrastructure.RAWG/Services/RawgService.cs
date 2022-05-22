using GamesLand.Infrastructure.RAWG.Entities;
using GamesLand.Infrastructure.RAWG.Requests;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace GamesLand.Infrastructure.RAWG.Services;

public class RawgService : IRawgService
{
    private readonly HttpClient _client;
    private readonly JsonSerializerSettings _jsonSerializerSettings;

    public RawgService(HttpClient client)
    {
        _client = client;
        _jsonSerializerSettings = new JsonSerializerSettings
        {
            ContractResolver = new DefaultContractResolver
            {
                NamingStrategy = new SnakeCaseNamingStrategy()
            }
        };
    }

    public async Task<IEnumerable<RawgGame>> SearchAsync(SearchRequest searchRequest)
    {
        var res =
            await _client.GetStringAsync("games" + AddSearchParams(searchRequest));
        var games = JsonConvert.DeserializeObject<RawgGameResults>(res, _jsonSerializerSettings);
        return games?.Results ?? Array.Empty<RawgGame>();
    }

    public async Task<RawgGame?> GetGame(int gameId)
    {
        var res = await _client.GetStringAsync($"games/{gameId}");
        var game = JsonConvert.DeserializeObject<RawgGame>(res, _jsonSerializerSettings);
        return game;
    }

    private string AddSearchParams(SearchRequest searchRequest)
    {
        var res = "?";
        if (searchRequest.Page > 0) res += $"page={searchRequest.Page}";
        if (searchRequest.PageSize > 0) res += $"page_size={searchRequest.PageSize}";
        if (searchRequest.Platforms.Any()) res += $"platforms={string.Join(",", searchRequest.Platforms)}";
        if (searchRequest.Name.Length > 0) res += $"search={searchRequest.Name}";

        return res.Length > 1 ? res : "";
    }
}