using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Learnz.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class TestEnd : Controller
{
    private readonly DataContext _dataContext;
    private readonly IUserService _userService;
    public TestEnd(DataContext dataContext, IUserService userService)
    {
        _dataContext = dataContext;
        _userService = userService;
    }

    [HttpPost]
    public async Task<ActionResult> EndTest(TestIdDTO request)
    {
        var guid = _userService.GetUserGuid();
        var test = await _dataContext.TestOfUsers.FirstOrDefaultAsync(tou => tou.UserId == guid && tou.TestId == request.TestId && tou.Ended == null);
        if (test == null)
        {
            return BadRequest(ErrorKeys.TestNotAccessible);
        }
        test.Ended = DateTime.UtcNow;
        await _dataContext.SaveChangesAsync();
        return Ok();
    }
}