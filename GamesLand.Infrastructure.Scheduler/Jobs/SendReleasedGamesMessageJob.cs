using GamesLand.Core.Games.Services;
using GamesLand.Core.Platforms.Services;
using GamesLand.Core.UserGames.Services;
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
    private readonly IPlatformsService _platformsService;
    private readonly IRawgService _rawgService;
    private readonly ITelegramService _telegramService;
    private readonly IUserGameService _userGameService;

    public SendReleasedGamesMessageJob(
        ILogger<SendReleasedGamesMessageJob> logger,
        IGamesService gamesService,
        IUserGameService userGameService,
        IPlatformsService platformsService,
        IRawgService rawgService,
        ITelegramService telegramService)
    {
        _logger = logger;
        _gamesService = gamesService;
        _userGameService = userGameService;
        _platformsService = platformsService;
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
            var rawgGame = await _rawgService.GetGame(g.ExternalId);
            await _gamesService.UpdateGameAsync(g.Id, rawgGame.ToGame());
            var externalPlatforms = rawgGame.Platforms?.Select(x => x.Platform);
            var platforms = await Task.WhenAll(externalPlatforms.Select(x =>
                _platformsService.GetPlatformByExternalIdAsync(x.Id)).ToList());
            foreach (var platform in platforms)
            {
                var releaseDateStr = externalPlatforms.FirstOrDefault(x => x.Id == platform.ExternalId)?.ReleasedAt;
                if (releaseDateStr == null || platform?.Id == null) continue;
                var releaseDate = DateTime.ParseExact(releaseDateStr, "yyyy-MM-dd", null);
                await _userGameService.UpdateReleaseDateAsync(g.Id, platform.Id, releaseDate);
            }
        }

        var usersGames = await _gamesService.GetReleasedUsersGameGroupedByUserIdAsync();

        await Task.WhenAll(usersGames.Select(x => _telegramService.SendMessageAsync(x)));
        await Task.WhenAll(usersGames.Select(x =>
            _gamesService.ChangeReleasedGameStatusAsync(x.User.Id, x.Id, x.Platform.Id, true)));
        _logger.LogInformation($"{gamesFound} email{(gamesFound > 1 ? "s" : "")} sent");
    }
}