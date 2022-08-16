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
        return Ok();
    }
}