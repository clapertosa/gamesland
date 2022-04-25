using Microsoft.Extensions.Configuration;

namespace GamesLand.Infrastructure.RAWG.Handlers;

public class RawgHttpMessageHandler : DelegatingHandler
{
    private readonly IConfiguration _configuration;

    public RawgHttpMessageHandler(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        char prefix = request.RequestUri != null && request.RequestUri.ToString().Contains('?') ? '&' : '?';
        Uri uriWithKey = new Uri(request.RequestUri + $"{prefix}key={_configuration["rawg_key"]}");
        request.RequestUri = uriWithKey;
        return base.SendAsync(request, cancellationToken);
    }
}