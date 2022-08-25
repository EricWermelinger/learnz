using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Learnz.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class TestClosed : Controller
{
    private readonly DataContext _dataContext;
    private readonly IUserService _userService;
    public TestClosed(DataContext dataContext, IUserService userService)
    {
        _dataContext = dataContext;
        _userService = userService;
    }

    [HttpGet]
    public async Task<ActionResult<List<TestDTO>>> GetClosedTests()
    {
        var guid = _userService.GetUserGuid();
        var tests = await _dataContext.Tests.Where(tst => (tst.OwnerId == guid || tst.TestOfUsers.Any(tou => tou.UserId == guid))
                                                && !tst.Active
                                                && (tst.OwnerId == guid || tst.Visible))
                                            .OrderByDescending(tst => tst.TestOfUsers.Any(tou => tou.UserId == guid) ? tst.TestOfUsers.First(tou => tou.UserId == guid).Started : tst.Created)
                                            .Select(tst => new TestDTO
                                            {
                                                TestId = tst.Id,
                                                TestOfUserId = tst.TestOfUsers.Any(tou => tou.UserId == guid) ? tst.TestOfUsers.First(tou => tou.UserId == guid).Id : null,
                                                Name = tst.Name,
                                                SubjectMain = tst.Set.SubjectMain,
                                                SubjectSecond = tst.Set.SubjectSecond,
                                                MaxTime = tst.MaxTime,
                                                IsOwner = tst.OwnerId == guid,
                                                IsGroupTest = tst.TestGroups.Any(),
                                                PointsScored = tst.TestOfUsers.Any(tou => tou.UserId == guid) ? tst.TestOfUsers.First(tou => tou.UserId == guid).TestQuestionOfUsers.Sum(tqs => tqs.PointsScored) : null,
                                                PointsPossible = tst.TestQuestions.Sum(tqs => tqs.PointsPossible)
                                            })
                                            .ToListAsync();
        return Ok(tests);
    }
}
