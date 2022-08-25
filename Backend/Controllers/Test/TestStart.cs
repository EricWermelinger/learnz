using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Learnz.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class TestStart : Controller
{
    private readonly DataContext _dataContext;
    private readonly IUserService _userService;
    public TestStart(DataContext dataContext, IUserService userService)
    {
        _dataContext = dataContext;
        _userService = userService;
    }

    [HttpPost]
    public async Task<ActionResult<TestIdDTO>> StartTest(TestIdDTO request)
    {
        var guid = _userService.GetUserGuid();
        var test = await _dataContext.Tests.FirstOrDefaultAsync(tst => tst.Id == request.TestId
                                                                && !tst.TestOfUsers.Any(tou => tou.UserId == guid)
                                                                && (tst.OwnerId == guid || (tst.TestGroups.Any() && tst.TestGroups.First().Group.GroupMembers.Any(grm => grm.UserId == guid)))
                                                                && tst.Visible
                                                                && tst.Active);
        if (test == null)
        {
            return BadRequest(ErrorKeys.TestNotAccessible);
        }

        var testOfUserId = Guid.NewGuid();
        var testOfUser = new TestOfUser
        {
            Id = testOfUserId,
            Started = DateTime.UtcNow,
            Ended = null,
            TestId = request.TestId,
            UserId = guid
        };
        _dataContext.TestOfUsers.Add(testOfUser);
        await _dataContext.SaveChangesAsync();

        var questions = await _dataContext.TestQuestions.Where(tqs => tqs.TestId == request.TestId).ToListAsync();
        foreach (var question in questions)
        {
            if (question != null)
            {
                var userQuestion = new TestQuestionOfUser
                {
                    TestOfUserId = testOfUserId,
                    TestQuestionId = question.Id,
                };
                _dataContext.TestQuestionOfUsers.Add(userQuestion);
            }
        }
        await _dataContext.SaveChangesAsync();

        var testDto = new TestIdDTO
        {
            TestId = testOfUserId
        };
        
        return Ok(testDto);
    }
}