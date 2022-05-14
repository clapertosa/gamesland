using GamesLand.Core.Games.Entities;
using GamesLand.Core.Games.Services;
using GamesLand.Infrastructure.RAWG.Entities;
using GamesLand.Infrastructure.RAWG.Services;
using Quartz;

namespace GamesLand.Infrastructure.Scheduler.Jobs;

[DisallowConcurrentExecution]
public class SendReleasedGamesMailJob : IJob
{
    private readonly IGamesService _gamesService;
    private readonly IRawgService _rawgService;

    public SendReleasedGamesMailJob(IGamesService gamesService, IRawgService rawgService)
    {
        _gamesService = gamesService;
        _rawgService = rawgService;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        IEnumerable<Game> games = await _gamesService.GetUsersGamesAsync();
        foreach (Game g in games)
        {
            RawgGame? game = await _rawgService.GetGame(g.ExternalId);
            await _gamesService.UpdateGameAsync(g.Id, game.ToGame());
        }

        //TODO if game is out, send an email
    }
}