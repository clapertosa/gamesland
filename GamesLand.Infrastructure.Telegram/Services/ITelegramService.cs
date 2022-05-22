using GamesLand.Core.Games.Entities;

namespace GamesLand.Infrastructure.Telegram.Services;

public interface ITelegramService
{
    Task SendMessageAsync(Game game);
}