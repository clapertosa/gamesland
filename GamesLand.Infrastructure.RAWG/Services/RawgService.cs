namespace GamesLand.Infrastructure.RAWG.Services;

public class RawgService : IRawgService
{
    private readonly HttpClient _client;

    public RawgService(HttpClient client)
    {
        _client = client;
    }
}