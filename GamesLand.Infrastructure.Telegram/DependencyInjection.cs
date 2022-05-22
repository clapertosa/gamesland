using GamesLand.Infrastructure.Telegram.Configuration;
using GamesLand.Infrastructure.Telegram.Services;
using Microsoft.Extensions.DependencyInjection;

namespace GamesLand.Infrastructure.Telegram;

public static class DependencyInjection
{
    public static void AddTelegram(this IServiceCollection services)
    {
        services.AddHostedService<ConfigureWebhook>();
        services.AddScoped<HandleBotUpdateService>();
        services.AddScoped<ITelegramService, TelegramService>();
    }
}