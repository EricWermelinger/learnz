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
    public LearnQuestionAnswerWrite(DataContext dataContext, IUserService userService)
    {
        _dataContext = dataContext;
        _userService = userService;
    }

    [HttpGet]
    public async Task<ActionResult<LearnSolutionDTO>> GetSolution(Guid learnSessionId,Guid questionId)
    {
        return Ok();
    }

    [HttpPost]
    public async Task<ActionResult> AnswerQuestion(LearnAnswerDTO request)
    {
        return Ok();
    }
}