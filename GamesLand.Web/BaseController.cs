using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GamesLand.Web;

[ApiController]
[Authorize]
[Route("api/[controller]")]
public class BaseController : ControllerBase
{
}