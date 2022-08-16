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
        return Ok();
    }
}