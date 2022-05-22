using GamesLand.Core.Games.Entities;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;

namespace GamesLand.Infrastructure.Telegram.Services;

public class TelegramService : ITelegramService
{
    private readonly ITelegramBotClient _botClient;

    public TelegramService(ITelegramBotClient botClient)
    {
        _botClient = botClient;
    }

    public async Task SendMessageAsync(Game game)
    {
        var releaseDate = game.Platform.GameReleaseDate;
        var html =
            $"Hi{(game.User.FirstName != null ? $" {game.User.FirstName}" : "")}, <a href='{game.Website}'><b>{game.Name}</b></a> has been released in {releaseDate?.ToString("dd/MM/yyyy")} on <b>{game.Platform.Name}</b>!";

        if (game.BackgroundImagePath != null)
            await _botClient.SendPhotoAsync(game.User.TelegramChatId, game.BackgroundImagePath);
        await _botClient.SendTextMessageAsync(game.User.TelegramChatId, html, ParseMode.Html);
    }
}