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
        var guid = _userService.GetUserGuid();
        var openSessions = await _dataContext.LearnSessions.Where(lss => lss.UserId == guid && lss.Ended == null)
                                                             .Select(lss => new LearnSessionDTO
                                                             {
                                                                 LearnSessionId = lss.Id,
                                                                 Created = lss.Created,
                                                                 Ended = lss.Ended,
                                                                 SetId = lss.SetId,
                                                                 SetName = lss.Set.Name,
                                                                 SubjectMain = lss.Set.SubjectMain,
                                                                 SubjectSecond = lss.Set.SubjectSecond
                                                             })
                                                             .ToListAsync();
        return Ok(openSessions);
    }

    [HttpPost]
    public async Task<ActionResult> OpenSession(LearnOpenNewSessionDTO request)
    {
        var guid = _userService.GetUserGuid();
        var alreadyOpened = await _dataContext.LearnSessions.FirstOrDefaultAsync(lss => lss.UserId == guid && lss.Ended == null && lss.SetId == request.SetId);
        if (alreadyOpened != null)
        {
            return BadRequest(ErrorKeys.LearnSessionAlreadyExists)
        }
        return Ok();
    }
}