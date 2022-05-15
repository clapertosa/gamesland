using GamesLand.Core.Errors;
using GamesLand.Core.Games.Entities;
using Microsoft.Extensions.Configuration;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace GamesLand.Infrastructure.MailSender.Services;

public class MailService : IMailService
{
    private readonly SendGridClient _sendGridClient;
    private readonly EmailAddress _emailAddress;

    public MailService(IConfiguration configuration)
    {
        _sendGridClient = new SendGridClient(configuration["sendgrid_key"]);
        _emailAddress = new EmailAddress(configuration["sendgrid_mail"]);
    }

    public async Task SendGameReleasedMailAsync(Game game)
    {
        EmailAddress emailTo = new EmailAddress(game.User.Email);
        string subject = $"\"{game.Name}\" is out!";
        DateTime? releaseDate = game.Platform.GameReleaseDate;

        string html = @"<!DOCTYPE html>
<html lang='en'>
  <head>
    <meta charset='UTF-8' />
    <meta http-equiv='X-UA-Compatible' content='IE=edge' />
    <meta name='viewport' content='width=device-width, initial-scale=1.0' />
  </head>
  <body style='margin: 0; padding: 0'>
    <div
      style='
        display: flex;
        justify-content: center;
        flex-flow: column;
        align-items: center;
      '
    >
      <h1 style='margin-top: 0px'>{{title}}</h1>
      <div style='width: 100%; height: auto'>
        <img
          style='width: 100%; height: 100%'
          src='{{backgroundImage}}'
          alt='{{title}}'s background image'
        />
      </div>
      <div style='font-size: 1.2rem; margin-top: 10px'>
        Hi{{firstName}}, <span style='font-weight: bold'>{{title}}</span> was released on
        {{releaseDate}} for {{platform}}
      </div>
      <br />
      <div><a href='{{website}}'>{{title}} website</a></div>
    </div>
  </body>
</html>
";
        html = html.Replace("{{title}}", game.Name)
            .Replace("{{firstName}}", game.User.FirstName != null ? ($" {game.User.FirstName}") : "")
            .Replace("{{releaseDate}}", releaseDate?.ToString("dd/MM/yyyy"))
            .Replace("{{platform}}", game.Platform.Name)
            .Replace("{{backgroundImage}}", game.BackgroundImagePath)
            .Replace("{{website}}", game.Website);
        var msg = MailHelper.CreateSingleEmail(_emailAddress, emailTo, subject, null, html);
        var response = await _sendGridClient.SendEmailAsync(msg);

        if (!response.IsSuccessStatusCode)
            throw new RestException(response.StatusCode, new { Message = await response.Body.ReadAsStringAsync() });
    }
}