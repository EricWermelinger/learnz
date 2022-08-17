using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Learnz.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class LearnQuestionAnswerWrite : Controller
{
    private readonly DataContext _dataContext;
    private readonly IUserService _userService;
    private readonly ILearnQueryService _learnQueryService;
    public LearnQuestionAnswerWrite(DataContext dataContext, IUserService userService, ILearnQueryService learnQueryService)
    {
        _dataContext = dataContext;
        _userService = userService;
        _learnQueryService = learnQueryService;
    }

    [HttpGet]
    public async Task<ActionResult<LearnSolutionDTO>> GetSolution(Guid learnSessionId,Guid questionId)
    {
        var guid = _userService.GetUserGuid();
        var question = await _dataContext.LearnQuestions.Where(lqs => lqs.LearnSessionId == learnSessionId && lqs.QuestionId == questionId && lqs.LearnSession.UserId == guid)
                                                        .FirstOrDefaultAsync();
        if (question == null)
        {
            return BadRequest(ErrorKeys.LearnSessionNotAccessible);
        }
        var solutionDto = new LearnSolutionDTO
        {
            Answer = _learnQueryService.GetAnswer(question)
        };
        return Ok();
    }

    [HttpPost]
    public async Task<ActionResult> AnswerQuestion(LearnAnswerDTO request)
    {
        var guid = _userService.GetUserGuid();
        var question = await _dataContext.LearnQuestions.Where(lqs => lqs.LearnSessionId == request.LearnSessionId && lqs.LearnSession.Ended == null && lqs.QuestionId == request.QuestionId && lqs.LearnSession.UserId == guid)
                                                        .FirstOrDefaultAsync();
        if (question == null)
        {
            return BadRequest(ErrorKeys.LearnSessionNotAccessible);
        }

        question.AnswerByUser = request.Answer;
        question.AnsweredCorrect = _learnQueryService.EvaluateAnswer(request.Answer, question.RightAnswer);
        bool questionLeft = await _dataContext.LearnQuestions.AnyAsync(lqs => lqs.LearnSessionId == request.LearnSessionId && lqs.AnsweredCorrect == null);
        if (!questionLeft)
        {
            var session = await _dataContext.LearnSessions.FirstAsync(lss => lss.Id == request.LearnSessionId);
            session.Ended = DateTime.UtcNow;
        }
        await _dataContext.SaveChangesAsync();
        return Ok();
    }
}