using GamesLand.Core.Games.Services;
using GamesLand.Infrastructure.RAWG.Services;
using GamesLand.Infrastructure.Telegram.Services;
using Microsoft.Extensions.Logging;
using Quartz;

namespace GamesLand.Infrastructure.Scheduler.Jobs;

[DisallowConcurrentExecution]
public class SendReleasedGamesMessageJob : IJob
{
    private readonly IGamesService _gamesService;
    private readonly ILogger<SendReleasedGamesMessageJob> _logger;
    private readonly IRawgService _rawgService;
    private readonly ITelegramService _telegramService;

    public SendReleasedGamesMessageJob(
        ILogger<SendReleasedGamesMessageJob> logger,
        IGamesService gamesService,
        IRawgService rawgService, ITelegramService telegramService)
    {
        _logger = logger;
        _gamesService = gamesService;
        _rawgService = rawgService;
        _telegramService = telegramService;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        _logger.LogInformation("SendReleasedGamesJob started");
        var games = await _gamesService.GetUsersGameGroupedByGameIdAsync();
        var gamesFound = games.Count();
        _logger.LogInformation($"{gamesFound} games found");
        foreach (var g in games)
        {
            var game = await _rawgService.GetGame(g.ExternalId);
            await _gamesService.UpdateGameAsync(g.Id, game.ToGame());
        }

        var usersGames = await _gamesService.GetReleasedUsersGameGroupedByUserIdAsync();

        await Task.WhenAll(usersGames.Select(x => _telegramService.SendMessageAsync(x)));
        await Task.WhenAll(usersGames.Select(x =>
            _gamesService.ChangeReleasedGameStatusAsync(x.User.Id, x.Id, x.Platform.Id, true)));
        _logger.LogInformation($"{gamesFound} email{(gamesFound > 1 ? "s" : "")} sent");
    }
}