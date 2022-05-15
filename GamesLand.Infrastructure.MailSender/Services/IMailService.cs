using GamesLand.Core.Games.Entities;

namespace GamesLand.Infrastructure.MailSender.Services;

public interface IMailService
{
    Task SendGameReleasedMailAsync(Game game);
}