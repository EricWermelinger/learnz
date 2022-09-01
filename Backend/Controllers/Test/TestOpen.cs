using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Learnz.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class TestOpen : Controller
{
    private readonly DataContext _dataContext;
    private readonly IUserService _userService;
    public TestOpen(DataContext dataContext, IUserService userService)
    {
        _dataContext = dataContext;
        _userService = userService;
    }

    [HttpGet]
    public async Task<ActionResult<List<TestDTO>>> GetOpenTests()
    {
        var guid = _userService.GetUserGuid();
        var tests = await _dataContext.Tests.Where(tst => (tst.OwnerId == guid || tst.TestGroups.Any(tgr => tgr.Group.GroupMembers.Any(grm => grm.UserId == guid)))
                                                && tst.Active
                                                && (tst.OwnerId == guid || tst.Visible)
                                                && !tst.TestOfUsers.Any(tou => tou.UserId == guid && tou.Ended != null))
                                            .OrderByDescending(tst => tst.TestGroups.Any() ? DateTime.MaxValue : tst.Created)
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
                                                PointsScored = null,
                                                PointsPossible = tst.TestQuestions.Where(tqs => tqs.Visible).Sum(tqs => tqs.PointsPossible)
                                            })
                                            .ToListAsync();
        return Ok(tests);
    }
}