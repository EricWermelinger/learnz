using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Learnz.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class TestResult : Controller
{
    private readonly DataContext _dataContext;
    private readonly IUserService _userService;
    private readonly ITestQueryService _testQueryService;
    public TestResult(DataContext dataContext, IUserService userService, ITestQueryService testQueryService)
    {
        _dataContext = dataContext;
        _userService = userService;
        _testQueryService = testQueryService;
    }

    [HttpGet]
    public async Task<ActionResult<TestResultDTO>> GetResult(Guid testOfUserId, Guid? userId = null)
    {
        var guid = _userService.GetUserGuid();
        var test = await _dataContext.TestOfUsers
            .Where(tou => (tou.UserId == guid && tou.Id == testOfUserId)
                            || (tou.Test.OwnerId == guid && tou.UserId == userId))
            .Include(tou => tou.Test)
            .ThenInclude(tqs => tqs.TestQuestions)
            .Include(tou => tou.Test)
            .ThenInclude(tqs => tqs.Set)
            .Include(tou => tou.TestQuestionOfUsers)
            .ThenInclude(tqu => tqu.TestQuestion)
            .FirstOrDefaultAsync();
        if (test == null)
        {
            return BadRequest(ErrorKeys.TestNotAccessible);
        }
        var result = new TestResultDTO
        {
            Name = test.Test.Name,
            MaxTime = test.Test.MaxTime,
            PointsPossible = test.Test.TestQuestions.Where(tqs => tqs.Visible).Sum(tqs => tqs.PointsPossible),
            PointsScored = test.TestQuestionOfUsers.Sum(tqu => tqu.PointsScored ?? 0),
            SubjectMain = test.Test.Set.SubjectMain,
            SubjectSecond = test.Test.Set.SubjectSecond,
            TimeUsed = test.Ended == null ? test.Test.MaxTime : Math.Min(((int)((TimeSpan)(test.Ended - test.Started)).TotalMinutes), test.Test.MaxTime),
            Questions = test.TestQuestionOfUsers.Select(tqs => new TestQuestionResultDTO
            {
                Answer = tqs.AnswerByUser,
                PointsScored = tqs.PointsScored ?? 0,
                PointsPossible = tqs.TestQuestion.PointsPossible,
                WasRight = tqs.AnsweredCorrect ?? false,
                Question = new GeneralQuestionQuestionDTO
                {
                    QuestionId = tqs.TestQuestion.QuestionId,
                    Question = tqs.TestQuestion.Question,
                    Description = tqs.TestQuestion.Description,
                    QuestionType = tqs.TestQuestion.QuestionType,
                    AnswerSetOne = tqs.TestQuestion.PossibleAnswers == null ? null : _testQueryService.GetAnswerSet(tqs.TestQuestion, true),
                    AnswerSetTwo = tqs.TestQuestion.PossibleAnswers == null ? null : _testQueryService.GetAnswerSet(tqs.TestQuestion, false)
                }
            }).ToList()
        };
        return Ok(result);
    }
}