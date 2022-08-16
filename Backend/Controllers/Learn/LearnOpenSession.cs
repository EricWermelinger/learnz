using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Learnz.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class LearnOpenSession : Controller
{
    private readonly DataContext _dataContext;
    private readonly IUserService _userService;
    public LearnOpenSession(DataContext dataContext, IUserService userService)
    {
        _dataContext = dataContext;
        _userService = userService;
    }

    [HttpGet]
    public async Task<ActionResult<List<LearnSessionDTO>>> GetOpenSessions()
    {
        return Ok();
    }

    [HttpPost]
    public async Task<ActionResult> OpenSession(LearnOpenNewSessionDTO request)
    {
        return Ok();
    }
}