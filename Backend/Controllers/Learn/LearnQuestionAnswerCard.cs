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
        
        var solutionDto = new LearnSolutionDTO
        {
            Answer = _learnQueryService.GetAnswer(question)
        };
        return Ok(solutionDto);
    }
}