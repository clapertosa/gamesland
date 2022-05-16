using GamesLand.Core.Games.Services;
using Quartz;

namespace GamesLand.Infrastructure.Scheduler.Jobs;

[DisallowConcurrentExecution]
public class DeleteNotifiedGamesJob : IJob
{
    private readonly IGamesService _gamesService;

    public DeleteNotifiedGamesJob(IGamesService gamesService)
    {
        _gamesService = gamesService;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        await _gamesService.DeleteNotifiedGamesAsync();
    }
}