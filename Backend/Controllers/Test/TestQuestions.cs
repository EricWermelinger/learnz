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
    public TestQuestions(DataContext dataContext, IUserService userService)
    {
        _dataContext = dataContext;
        _userService = userService;
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
                                                                      AnswerSetOne = tqu.TestQuestion.PossibleAnswers == null ? null : GetAnswerSet(tqu.TestQuestion, true),
                                                                      AnswerSetTwo = tqu.TestQuestion.PossibleAnswers == null ? null : GetAnswerSet(tqu.TestQuestion, false)
                                                                  },
                                                                  CurrentAnswer = tqu.AnswerByUser
                                                              })
                                                              .ToListAsync();

        return Ok(questions);
    }

    private List<ChallengeQuestionAnswerDTO>? GetAnswerSet(TestQuestion tqs, bool firstSet)
    {
        switch (tqs.QuestionType)
        {
            case QuestionType.Distribute:
                var answerSet = tqs.RightAnswer.Split("|||").Select(ans => ans.Split("||")[firstSet ? 0 : 1]).ToList();
                var answersDistribute = answerSet.Select(ans => new ChallengeQuestionAnswerDTO
                {
                    AnswerId = new Guid(ans.Split("|")[0]),
                    Answer = ans.Split("|")[1]
                })
                .OrderBy(ans => ans.Answer)
                .ToList();
                return answersDistribute;
            case QuestionType.MultipleChoice:
                if (firstSet)
                {
                    var answerMultipleChoice = tqs.RightAnswer.Split("||").Select(ans => new ChallengeQuestionAnswerDTO
                    {
                        AnswerId = new Guid(ans.Split("|")[0]),
                        Answer = ans.Split("|")[1]
                    })
                    .ToList();
                    return answerMultipleChoice;
                }
                return null;
        }
        return null;
    }
}