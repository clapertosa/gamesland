using Telegram.Bot.Exceptions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace GamesLand.Infrastructure.Telegram.Services;

public class HandleBotUpdateService
{
    public async Task EchoAsync(Update update)
    {
        var handler = update.Type switch
        {
            UpdateType.Message => BotOnMessageReceivedAsync(update.Message),
            UpdateType.CallbackQuery => BotOnCallbackQueryReceivedAsync(update.CallbackQuery),
            _ => UnknownUpdateHandlerAsync(update)
        };

        try
        {
            await handler;
        }
        catch (Exception exception)
        {
            await HandleErrorAsync(exception);
        }
    }

    private Task BotOnMessageReceivedAsync(Message updateMessage)
    {
        return Task.CompletedTask;
    }


    private Task BotOnCallbackQueryReceivedAsync(CallbackQuery updateCallbackQuery)
    {
        return Task.CompletedTask;
    }

    private Task UnknownUpdateHandlerAsync(Update update)
    {
        return Task.CompletedTask;
    }

    private Task HandleErrorAsync(Exception exception)
    {
        var errorMessage = exception switch
        {
            ApiRequestException apiRequestException =>
                $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
            _ => exception.ToString()
        };

        return Task.CompletedTask;
    }
}