using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Learnz.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class TestSettings : Controller
{
    private readonly DataContext _dataContext;
    private readonly IUserService _userService;
    private readonly ITestQueryService _testQueryService;
    public TestSettings(DataContext dataContext, IUserService userService, ITestQueryService testQueryService)
    {
        _dataContext = dataContext;
        _userService = userService;
        _testQueryService = testQueryService;
    }

    [HttpGet]
    public async Task<ActionResult<TestSettingDTO>> GetSettings(Guid testId)
    {
        var guid = _userService.GetUserGuid();
        var test = await _dataContext.Tests.FirstOrDefaultAsync(tst => tst.Id == testId
                                                                    && tst.OwnerId == guid);
        var groupTest = await _dataContext.TestGroups.FirstOrDefaultAsync(tst => tst.TestId == testId);
        if (test == null || groupTest == null)
        {
            return BadRequest(ErrorKeys.TestNotAccessible);
        }
        var setting = new TestSettingDTO
        {
            Active = test.Active,
            Name = test.Name,
            MaxTime = test.MaxTime,
            SetName = test.Set.Name,
            SubjectMain = test.Set.SubjectMain,
            SubjectSecond = test.Set.SubjectSecond,
            TestId = testId,
            Visible = test.Visible,
            Questions = test.TestQuestions.Select(tqs => new TestQuestionSettingDTO
            {
                Visible = tqs.Visible,
                PointsPossible = tqs.PointsPossible,
                Question = new GeneralQuestionQuestionDTO
                {
                    Question = tqs.Question,
                    QuestionId = tqs.QuestionId,
                    Description = tqs.Description,
                    QuestionType = tqs.QuestionType,
                    AnswerSetOne = tqs.PossibleAnswers == null ? null : _testQueryService.GetAnswerSet(tqs, true),
                    AnswerSetTwo = tqs.PossibleAnswers == null ? null : _testQueryService.GetAnswerSet(tqs, false),
                }
            }).ToList()
        };
        return Ok(setting);
    }

    [HttpPost]
    public async Task<ActionResult> UpdateSettings(TestSaveSettingsDTO request)
    {
        var guid = _userService.GetUserGuid();
        var test = await _dataContext.Tests.FirstOrDefaultAsync(tst => tst.OwnerId == guid && tst.Id == request.TestId);
        if (test == null)
        {
            return BadRequest(ErrorKeys.TestNotAccessible);
        }
        if (test.Visible && !request.Visible)
        {
            var timeStamp = DateTime.UtcNow;
            var notEndedTestOfUsers = await _dataContext.TestOfUsers.Where(tou => tou.TestId == test.Id && tou.Ended == null).ToListAsync();
            foreach (var testOfUser in notEndedTestOfUsers)
            {
                if (testOfUser != null)
                {
                    testOfUser.Ended = timeStamp;
                }
            }
            await _dataContext.SaveChangesAsync();
        }

        test.Name = request.Name;
        test.MaxTime = request.MaxTime;
        test.Visible = request.Visible;
        test.Active = !test.Active ? false : request.Active;

        var testQuestions = await _dataContext.TestQuestions.Where(tqs => tqs.TestId == test.Id).ToListAsync();
        foreach (var testQuestion in testQuestions)
        {
            var newValue = testQuestion == null ? null : request.Questions.FirstOrDefault(rqs => rqs.QuestionId == testQuestion.QuestionId);
            if (testQuestion == null || newValue == null)
            {
                continue;
            }
            testQuestion.Visible = newValue.Visible;
            testQuestion.PointsPossible = newValue.Points;
        }

        await _dataContext.SaveChangesAsync();

        return Ok();
    }
}
