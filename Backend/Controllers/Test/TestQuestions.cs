using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Learnz.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class TestQuestions : Controller
{
    private readonly DataContext _dataContext;
    private readonly IUserService _userService;
    private readonly ITestQueryService _testQueryService;
    public TestQuestions(DataContext dataContext, IUserService userService, ITestQueryService testQueryService)
    {
        _dataContext = dataContext;
        _userService = userService;
        _testQueryService = testQueryService;
    }

    [HttpGet]
    public async Task<ActionResult<List<TestQuestionDTO>>> GetQuestions(Guid testOfUserId)
    {
        var guid = _userService.GetUserGuid();
        var testOfUser = await _dataContext.TestOfUsers.FirstOrDefaultAsync(tou => tou.UserId == guid
                                                                            && tou.Id == testOfUserId
                                                                            && tou.Test.Visible
                                                                            && tou.Test.Active
                                                                            && tou.Ended == null
                                                                            && tou.Started.AddMinutes(tou.Test.MaxTime) > DateTime.UtcNow);
        if (testOfUser == null)
        {
            return BadRequest(ErrorKeys.TestNotAccessible);
        }
        
        var questions = await _dataContext.TestQuestionOfUsers.Where(tou => tou.TestOfUserId == testOfUserId && tou.TestQuestion.Visible)
                                                              .Select(tqu => new TestQuestionDTO
                                                              {
                                                                  Question = new GeneralQuestionQuestionDTO
                                                                  {
                                                                      QuestionId = tqu.TestQuestion.QuestionId,
                                                                      Question = tqu.TestQuestion.Question,
                                                                      Description = tqu.TestQuestion.Description,
                                                                      QuestionType = tqu.TestQuestion.QuestionType,
                                                                      AnswerSetOne = tqu.TestQuestion.PossibleAnswers == null ? null : _testQueryService.GetAnswerSet(tqu.TestQuestion, true),
                                                                      AnswerSetTwo = tqu.TestQuestion.PossibleAnswers == null ? null : _testQueryService.GetAnswerSet(tqu.TestQuestion, false)
                                                                  },
                                                                  Answer = tqu.AnswerByUser
                                                              })
                                                              .ToListAsync();

        return Ok(questions);
    }

    [HttpPost]
    public async Task<ActionResult> SaveAnswer(TestAnswerDTO request)
    {
        var guid = _userService.GetUserGuid();
        var question = await _dataContext.TestQuestionOfUsers.Include(tqu => tqu.TestQuestion)
                                                             .FirstOrDefaultAsync(tqu => tqu.Id == request.QuestionOfUserId
                                                                                   && tqu.TestOfUser.UserId == guid
                                                                                   && tqu.TestOfUser.Ended == null
                                                                                   && tqu.TestOfUser.Started.AddMinutes(tqu.TestOfUser.Test.MaxTime) > DateTime.UtcNow
                                                                                   && tqu.TestOfUser.Test.Active
                                                                                   && tqu.TestOfUser.Test.Visible);
        if (question == null)
        {
            return BadRequest(ErrorKeys.TestNotAccessible);
        }

        question.AnswerByUser = request.Answer;
        bool isCorrect = _testQueryService.EvaluateAnswer(request.Answer, question.TestQuestion.RightAnswer, question.TestQuestion.QuestionType);
        question.AnsweredCorrect = isCorrect;
        question.PointsScored = isCorrect ? question.TestQuestion.PointsPossible : 0;

        await _dataContext.SaveChangesAsync();
        return Ok();
    }
}