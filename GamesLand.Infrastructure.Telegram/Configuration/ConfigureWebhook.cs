using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;

namespace GamesLand.Infrastructure.Telegram.Configuration;

public class ConfigureWebhook : IHostedService
{
    private readonly string _botToken;
    private readonly string _host;
    private readonly ILogger<ConfigureWebhook> _logger;
    private readonly IServiceProvider _services;

    public ConfigureWebhook(
        ILogger<ConfigureWebhook> logger,
        IServiceProvider serviceProvider,
        IConfiguration configuration
    )
    {
        _logger = logger;
        _services = serviceProvider;
        _botToken = configuration["telegram_bot:token"];
        _host = configuration["telegram_bot:host"];
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        using var scope = _services.CreateScope();
        var botClient = scope.ServiceProvider.GetRequiredService<ITelegramBotClient>();

        var webhookAddress = @$"{_host}";
        _logger.LogInformation("Setting webhook: {webhookAddress}", webhookAddress);

        var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")?.ToUpper();
        if (env is "DEVELOPMENT" or "PRODUCTION")
            await botClient.SetWebhookAsync(
                webhookAddress,
                allowedUpdates: Array.Empty<UpdateType>(),
                cancellationToken: cancellationToken);
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        using var scope = _services.CreateScope();
        var botClient = scope.ServiceProvider.GetRequiredService<ITelegramBotClient>();

        // Remove webhook upon app shutdown
        _logger.LogInformation("Removing webhook");
        await botClient.DeleteWebhookAsync(cancellationToken: cancellationToken);
    }
}