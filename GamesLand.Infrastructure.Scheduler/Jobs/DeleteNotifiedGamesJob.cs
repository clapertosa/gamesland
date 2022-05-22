using GamesLand.Core.Games.Services;
using Microsoft.Extensions.Logging;
using Quartz;

namespace GamesLand.Infrastructure.Scheduler.Jobs;

[DisallowConcurrentExecution]
public class DeleteNotifiedGamesJob : IJob
{
    private readonly IGamesService _gamesService;
    private readonly ILogger<DeleteNotifiedGamesJob> _logger;

    public DeleteNotifiedGamesJob(ILogger<DeleteNotifiedGamesJob> logger, IGamesService gamesService)
    {
        _logger = logger;
        _gamesService = gamesService;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        _logger.LogInformation("DeleteNotifiedGamesJob started");
        await _gamesService.DeleteNotifiedGamesAsync();
        _logger.LogInformation("Notified games deleted");
    }
}