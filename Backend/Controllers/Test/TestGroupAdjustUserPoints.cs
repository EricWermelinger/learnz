using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Learnz.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class TestGroupAdjustUserPoints : Controller
{
    private readonly DataContext _dataContext;
    private readonly IUserService _userService;
    public TestGroupAdjustUserPoints(DataContext dataContext, IUserService userService)
    {
        _dataContext = dataContext;
        _userService = userService;
    }

    [HttpPost]
    public async Task<ActionResult> AdjustUserPoints(TestAdjustUserPointDTO request)
    {
        var guid = _userService.GetUserGuid();
        var testQuestion = await _dataContext.TestQuestionOfUsers.Where(tqu => tqu.TestOfUser.Test.OwnerId == guid
                                                                                && tqu.TestOfUser.Id == request.TestOfUserId
                                                                                && tqu.TestQuestion.QuestionId == request.QuestionId)
                                                                 .FirstOrDefaultAsync();
        if (testQuestion == null)
        {
            return BadRequest(ErrorKeys.TestNotAccessible);
        }
        
        testQuestion.PointsScored = request.PointsScored;
        testQuestion.AnsweredCorrect = request.IsCorrect;
        await _dataContext.SaveChangesAsync();

        return Ok();
    }
}