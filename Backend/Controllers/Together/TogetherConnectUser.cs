using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace Learnz.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class TogetherConnectUser : Controller
{
    private readonly IUserService _userService;
    private readonly ITogetherQueryService _togetherQueryService;
    private readonly HubService _hubService;
    public TogetherConnectUser(IUserService userService, ITogetherQueryService togetherQueryService, IHubContext<LearnzHub> learnzHub)
    {
        _userService = userService;
        _togetherQueryService = togetherQueryService;
        _hubService = new HubService(learnzHub);
    }

    [HttpGet]
    public async Task<ActionResult<List<TogetherOverviewUserProfileDTO>>> GetConnections()
    {
        var guid = _userService.GetUserGuid();
        var connections = await _togetherQueryService.GetConnectionOverview(guid);
        return Ok(connections);
    }
}
