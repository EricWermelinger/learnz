using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Learnz.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class TestResultOverview : Controller
{
    private readonly DataContext _dataContext;
    private readonly IUserService _userService;
    public TestResultOverview(DataContext dataContext, IUserService userService)
    {
        _dataContext = dataContext;
        _userService = userService;
    }

    [HttpGet]
    public async Task<ActionResult<TestResultOverviewDTO>> GetResultOverview(Guid testId)
    {
        var guid = _userService.GetUserGuid();
        var test = await _dataContext.Tests.Where(tst => tst.OwnerId == guid && tst.Id == testId).Include(tst => tst.TestQuestions).FirstOrDefaultAsync();
        if (test == null)
        {
            return BadRequest(ErrorKeys.TestNotAccessible);
        }
        var testResultUsers = await _dataContext.TestOfUsers.Where(tou => tou.TestId == testId)
                                                       .Select(tou => new TestResultOverviewUserDTO
                                                       {
                                                           Username = tou.User.Username,
                                                           UserId = tou.UserId,
                                                           TestOfUserId = tou.Id,
                                                           PointsScored = tou.TestQuestionOfUsers.Sum(tqu => tqu.PointsScored ?? 0),
                                                           TimeUsed = tou.Ended == null ? tou.Test.MaxTime : Math.Min(((int)((TimeSpan)(tou.Ended - tou.Started)).TotalMinutes), tou.Test.MaxTime),
                                                       })
                                                       .OrderByDescending(usr => usr.PointsScored)
                                                       .ToListAsync();
        var testResult = new TestResultOverviewDTO
        {
            TestName = test.Name,
            MaxTime = test.MaxTime,
            PointsPossible = test.TestQuestions.Where(tqs => tqs.Visible).Sum(tqs => tqs.PointsPossible),
            Results = testResultUsers
        };
        
        return Ok(testResult);
    }
}
