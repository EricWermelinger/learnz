using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Learnz.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class LearnQuestionAnswerCard : Controller
{
    private readonly DataContext _dataContext;
    private readonly IUserService _userService;
    private readonly ILearnQueryService _learnQueryService;
    public LearnQuestionAnswerCard(DataContext dataContext, IUserService userService, ILearnQueryService learnQueryService)
    {
        _dataContext = dataContext;
        _userService = userService;
        _learnQueryService = learnQueryService;
    }

    [HttpGet]
    public async Task<ActionResult<LearnSolutionDTO>> GetNextSolution(Guid learnSessionId, Guid questionId)
    {
        var guid = _userService.GetUserGuid();
        var question = await _dataContext.LearnQuestions.Where(lqs => lqs.LearnSessionId == learnSessionId && lqs.LearnSession.UserId == guid && lqs.QuestionId == questionId)
                                                        .FirstOrDefaultAsync();
        if (question == null)
        {
            return BadRequest(ErrorKeys.LearnSessionNotAccessible);
        }

        question.AnswerByUser = "";
        if (question.AnsweredCorrect != false)
        {
            question.AnsweredCorrect = true;
        }
        await _dataContext.SaveChangesAsync();

        var sessionWithQuestionLeft = await _dataContext.LearnSessions.FirstOrDefaultAsync(lss => lss.Id == learnSessionId && lss.UserId == guid && lss.Questions.Any(lqs => lqs.AnsweredCorrect == null));
        if (sessionWithQuestionLeft != null)
        {
            sessionWithQuestionLeft.Ended = DateTime.UtcNow;
            await _dataContext.SaveChangesAsync();
        }

        var solutionDto = new LearnSolutionDTO
        {
            Answer = _learnQueryService.GetAnswer(question),
            WasCorrect = true
        };
        return Ok(solutionDto);
    }
}