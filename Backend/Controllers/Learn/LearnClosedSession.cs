using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Learnz.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class LearnClosedSession : Controller
{
    private readonly DataContext _dataContext;
    private readonly IUserService _userService;
    public LearnClosedSession(DataContext dataContext, IUserService userService)
    {
        _dataContext = dataContext;
        _userService = userService;
    }

    [HttpGet]
    public async Task<ActionResult<List<LearnSessionDTO>>> GetClosedSessions()
    {
        var guid = _userService.GetUserGuid();
        return Ok();
    }
}