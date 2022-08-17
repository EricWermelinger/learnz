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
        var closedSessions = await _dataContext.LearnSessions.Where(lss => lss.UserId == guid && lss.Ended != null)
                                                             .Select(lss => new LearnSessionDTO
                                                             {
                                                                 LearnSessionId = lss.Id,
                                                                 Created = lss.Created,
                                                                 Ended = lss.Ended,
                                                                 SetId = lss.SetId,
                                                                 SetName = lss.Set.Name,
                                                                 SubjectMain = lss.Set.SubjectMain,
                                                                 SubjectSecond = lss.Set.SubjectSecond,
                                                                 NumberOfRightAnswers = lss.Questions.Where(lqs => lqs.AnsweredCorrect == true).Count(),
                                                                 NumberOfWrongAnswers = lss.Questions.Where(lqs => lqs.AnsweredCorrect == false).Count(),
                                                                 NumberOfNotAnswerd = lss.Questions.Where(lqs => lqs.AnsweredCorrect == null).Count()
                                                             })
                                                             .ToListAsync();
        return Ok(closedSessions);
    }
}