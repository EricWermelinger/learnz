using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Learnz.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class LearnQuestionSetCorrect : Controller
{
    private readonly DataContext _dataContext;
    private readonly IUserService _userService;
    public LearnQuestionSetCorrect(DataContext dataContext, IUserService userService)
    {
        _dataContext = dataContext;
        _userService = userService;
    }

    [HttpPost]
    public async Task<ActionResult> AnswerQuestion(LearnQuestionSetCorrectDTO request)
    {
        var guid = _userService.GetUserGuid();
        var question = await _dataContext.LearnQuestions.Where(lqs => lqs.LearnSessionId == request.LearnSessionId && lqs.LearnSession.Ended == null && lqs.QuestionId == request.QuestionId && lqs.LearnSession.UserId == guid)
                                                        .FirstOrDefaultAsync();
        if (question == null)
        {
            return BadRequest(ErrorKeys.LearnSessionNotAccessible);
        }

        question.AnsweredCorrect = request.Correct;
        await _dataContext.SaveChangesAsync();
        return Ok();
    }
}