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
        var testOfUser = await _dataContext.TestOfUsers.FirstOrDefaultAsync(tou => tou.UserId == guid && tou.Id == request.TestId && tou.Ended == null);
        if (testOfUser == null)
        {
            return BadRequest(ErrorKeys.TestNotAccessible);
        }
        var test = await _dataContext.Tests.FirstAsync(tst => tst.Id == testOfUser.TestId);
        DateTime maxEnding = testOfUser.Started.AddMinutes(test.MaxTime);
        testOfUser.Ended = DateTime.UtcNow > maxEnding ? maxEnding : DateTime.UtcNow;
        await _dataContext.SaveChangesAsync();
        var isGroupTest = await _dataContext.TestGroups.AnyAsync(tgr => tgr.TestId == testOfUser.TestId);
        if (!isGroupTest)
        {
            test.Active = false;
            await _dataContext.SaveChangesAsync();
        }
        return Ok();
    }
}