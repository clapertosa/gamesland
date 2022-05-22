using GamesLand.Infrastructure.Telegram.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Telegram.Bot.Types;

namespace GamesLand.Web.Telegram.Controllers;

public class TelegramController : BaseController
{
    [AllowAnonymous]
    [HttpPost]
    public async Task<ActionResult> Index(
        [FromServices] HandleBotUpdateService handleUpdateService,
        [FromBody] Update? update
    )
    {
        await handleUpdateService.EchoAsync(update);
        return new OkResult();
    }
}