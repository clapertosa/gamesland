using GamesLand.Core.Games.Entities;
using GamesLand.Core.Games.Services;
using GamesLand.Infrastructure.MailSender.Services;
using GamesLand.Infrastructure.RAWG.Entities;
using GamesLand.Infrastructure.RAWG.Services;
using Quartz;

namespace GamesLand.Infrastructure.Scheduler.Jobs;

[DisallowConcurrentExecution]
public class SendReleasedGamesMailJob : IJob
{
    private readonly IGamesService _gamesService;
    private readonly IRawgService _rawgService;
    private readonly IMailService _mailService;

    public SendReleasedGamesMailJob(IGamesService gamesService, IRawgService rawgService, IMailService mailService)
    {
        _gamesService = gamesService;
        _rawgService = rawgService;
        _mailService = mailService;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        IEnumerable<Game> games = await _gamesService.GetUsersGameGroupedByGameIdAsync();
        foreach (Game g in games)
        {
            RawgGame? game = await _rawgService.GetGame(g.ExternalId);
            await _gamesService.UpdateGameAsync(g.Id, game.ToGame());
        }

        IEnumerable<Game> usersGames = await _gamesService.GetReleasedUsersGameGroupedByUserIdAsync();

        await Task.WhenAll(usersGames.Select(x => _mailService.SendGameReleasedMailAsync(x)));
        await Task.WhenAll(usersGames.Select(x =>
            _gamesService.ChangeReleasedGameStatusAsync(x.User.Id, x.Id, x.Platform.Id, true)));
    }
}