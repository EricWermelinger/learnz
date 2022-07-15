using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Learnz.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class WebSocketConnections : Controller
{
    private readonly IUserService _userService;
    public WebSocketConnections(IUserService userService)
    {
        _userService = userService;
    }

    [HttpPost]
    public ActionResult AddConnection(WebSocketConnectionDTO request)
    {
        var userId = _userService.GetUserGuid();
        HubConnections.EditConnection(request.ConnectionId, userId);
        return Ok();
    }
}
