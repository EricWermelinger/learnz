using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Learnz.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class LearnSessionQuestions : Controller
{
    private readonly DataContext _dataContext;
    private readonly IUserService _userService;
    public LearnSessionQuestions(DataContext dataContext, IUserService userService)
    {
        _dataContext = dataContext;
        _userService = userService;
    }

    [HttpGet]
    public async Task<ActionResult<List<LearnQuestionDTO>>> GetQuestions(Guid learnSessionId)
    {
        return Ok();
    }
}