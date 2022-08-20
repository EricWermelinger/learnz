using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Learnz.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class LearnMarkQuestion : Controller
{
    private readonly DataContext _dataContext;
    private readonly IUserService _userService;
    public LearnMarkQuestion(DataContext dataContext, IUserService userService)
    {
        _dataContext = dataContext;
        _userService = userService;
    }

    [HttpPost]
    public async Task<ActionResult> MarkQuestion(LearnMarkQuestionDTO request)
    {
        var guid = _userService.GetUserGuid();
        var question = await _dataContext.LearnQuestions.Where(lqs => lqs.LearnSessionId == request.LearnSessionId && lqs.QuestionId == request.QuestionId && lqs.LearnSession.UserId == guid)
                                                        .FirstOrDefaultAsync();
        if (question == null)
        {
            return BadRequest(ErrorKeys.LearnSessionNotAccessible);
        }
        question.MarkedAsHard = request.Hard;
        await _dataContext.SaveChangesAsync();
        return Ok();
    }
}