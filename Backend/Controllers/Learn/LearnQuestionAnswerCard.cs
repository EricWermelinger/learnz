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
    public LearnQuestionAnswerCard(DataContext dataContext, IUserService userService)
    {
        _dataContext = dataContext;
        _userService = userService;
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
        string answer = "";
        switch (question.QuestionType)
        {
            case QuestionType.Distribute:
                answer = string.Join(" & ", question.RightAnswer.Split("|||").Select(ans => ans.Split("||")[0].Split("|")[1] + " - " + ans.Split("||")[1].Split("|")[1]));
                break;
            case QuestionType.MultipleChoice:
                answer = string.Join(" & ", question.RightAnswer.Split("||").Select(ans => ans.Split("|")[1]));
                break;
            case QuestionType.Mathematic:
            case QuestionType.OpenQuestion:
            case QuestionType.TrueFalse:
            case QuestionType.Word:
                answer = question.RightAnswer;
                break;
        }
        var solutionDto = new LearnSolutionDTO
        {
            Answer = answer
        };
        return Ok(solutionDto);
    }
}