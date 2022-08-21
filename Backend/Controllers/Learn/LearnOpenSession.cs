using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace Learnz.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class LearnOpenSession : Controller
{
    private readonly DataContext _dataContext;
    private readonly IUserService _userService;
    private readonly ISetPolicyChecker _setPolicyChecker;
    public LearnOpenSession(DataContext dataContext, IUserService userService, ISetPolicyChecker setPolicyChecker)
    {
        _dataContext = dataContext;
        _userService = userService;
        _setPolicyChecker = setPolicyChecker;
    }

    [HttpGet]
    public async Task<ActionResult<List<LearnSessionDTO>>> GetOpenSessions()
    {
        var guid = _userService.GetUserGuid();
        var openSessions = await _dataContext.LearnSessions.Where(lss => lss.UserId == guid && lss.Ended == null)
                                                             .Select(lss => new LearnSessionDTO
                                                             {
                                                                 LearnSessionId = lss.Id,
                                                                 Created = lss.Created,
                                                                 Ended = lss.Ended,
                                                                 SetId = lss.SetId,
                                                                 SetName = lss.Set.Name,
                                                                 SubjectMain = lss.Set.SubjectMain,
                                                                 SubjectSecond = lss.Set.SubjectSecond,
                                                                 NumberOfRightAnswers = lss.Questions.Where(lqs => lqs.AnsweredCorrect == true).Count(),
                                                                 NumberOfWrongAnswers = lss.Questions.Where(lqs => lqs.AnsweredCorrect == false).Count(),
                                                                 NumberOfNotAnswerd = lss.Questions.Where(lqs => lqs.AnsweredCorrect == null).Count()
                                                             })
                                                             .OrderByDescending(lss => lss.Created)
                                                             .ToListAsync();
        return Ok(openSessions);
    }

    [HttpPost]
    public async Task<ActionResult> OpenSession(LearnOpenNewSessionDTO request)
    {
        var guid = _userService.GetUserGuid();
        var alreadyOpened = await _dataContext.LearnSessions.FirstOrDefaultAsync(lss => lss.UserId == guid && lss.Ended == null && lss.SetId == request.SetId);
        var alreadyExistend = await _dataContext.LearnSessions.AnyAsync(lss => lss.Id == request.LearnSessionId);
        if (alreadyOpened != null || alreadyExistend)
        {
            return BadRequest(ErrorKeys.LearnSessionAlreadyExists);
        }
        
        var set = await _dataContext.CreateSets
            .Where(crs => crs.Id == request.SetId)
            .Include(crs => crs.QuestionDistributes)
            .ThenInclude(qd => qd.Answers)
            .Include(crs => crs.QuestionMathematics)
            .ThenInclude(qm => qm.Variables)
            .Include(crs => crs.QuestionMultipleChoices)
            .ThenInclude(qmc => qmc.Answers)
            .Include(crs => crs.QuestionOpenQuestions)
            .Include(crs => crs.QuestionTrueFalses)
            .Include(crs => crs.QuestionWords)
            .FirstOrDefaultAsync();
        if (set == null || !_setPolicyChecker.SetUsable(set, guid))
        {
            return BadRequest(ErrorKeys.SetNotAccessible);
        }

        List<Guid> hardQuestionIds = new List<Guid>();
        if (request.OnlyHardQuestions)
        {
            var lastSession = await _dataContext.LearnSessions.Where(lss => lss.UserId == guid && lss.SetId == request.SetId)
                                                               .OrderByDescending(lss => lss.Ended)
                                                               .Include(lss => lss.Questions)
                                                               .FirstOrDefaultAsync();
            if (lastSession != null)
            {
                hardQuestionIds.AddRange(lastSession.Questions.Where(lqs => lqs.MarkedAsHard == true).Select(lqs => lqs.QuestionId));
            }
            var difficultQuestions = await _dataContext.LearnQuestions.Where(lqs => lqs.LearnSession.UserId == guid && lqs.LearnSession.SetId == request.SetId && lqs.AnsweredCorrect != null)
                                                                      .Select(lqs => new { QuestionId = lqs.QuestionId, Correct = lqs.AnsweredCorrect == true ? 1 : 0 })
                                                                      .GroupBy(lqs => lqs.QuestionId)
                                                                      .Select(qst => new { QuestionId = qst.Key, Correct = qst.Sum(lqs => lqs.Correct), Answered = qst.Count() })
                                                                      .Where(qst => ((double)qst.Correct / (double)qst.Answered) < 0.5)
                                                                      .Select(qst => qst.QuestionId)
                                                                      .ToListAsync();
            hardQuestionIds.AddRange(difficultQuestions);
        }
        if (request.OnlyHardQuestions && hardQuestionIds.Count == 0)
        {
            return BadRequest(ErrorKeys.LearnSetContainsNoHardQuesitons); 
        }

        var session = new LearnSession
        {
            Id = request.LearnSessionId,
            Created = DateTime.UtcNow,
            SetId = request.SetId,
            UserId = guid
        };
        _dataContext.LearnSessions.Add(session);
        await _dataContext.SaveChangesAsync();

        foreach (var question in set.QuestionDistributes)
        {
            if (request.OnlyHardQuestions && !hardQuestionIds.Contains(question.Id))
            {
                continue;
            }
            var newQuestion = new LearnQuestion
            {
                LearnSessionId = request.LearnSessionId,
                QuestionId = question.Id,
                Question = question.Question,
                QuestionType = QuestionType.Distribute,
                PossibleAnswers = string.Join("|||", question.Answers.Select(ans => ans.LeftSideId.ToString() + "|" + ans.LeftSide + "||" + ans.RightSideId.ToString() + "|" + ans.RightSide)),
                RightAnswer = string.Join("|||", question.Answers.Select(ans => ans.LeftSideId.ToString() + "|" + ans.LeftSide + "||" + ans.RightSideId.ToString() + "|" + ans.RightSide)),
                MarkedAsHard = hardQuestionIds.Contains(question.Id)
            };
            _dataContext.LearnQuestions.Add(newQuestion);
        }
        foreach (var question in set.QuestionMathematics)
        {
            if (request.OnlyHardQuestions && !hardQuestionIds.Contains(question.Id))
            {
                continue;
            }
            string mathematicQuestion = question.Question;
            string mathematicAnswer = question.Answer;
            string answerComputed = "";
            Random random = new();
            foreach (var variable in question.Variables)
            {
                if (variable != null)
                {
                    int steps = (int)Math.Floor((variable.Max - variable.Min) / variable.Interval == 0 ? 1 : variable.Interval);
                    int randomStep = random.Next(0, steps);
                    double variableValue = Math.Round(variable.Min + randomStep * variable.Interval, variable.Digits);
                    mathematicQuestion = mathematicQuestion.Replace(variable.Display, variableValue.ToString());
                    mathematicAnswer = mathematicAnswer.Replace(variable.Display, variableValue.ToString());
                }
            }
            try
            {
                var mathematicAnswerObj = new DataTable().Compute(mathematicAnswer, "");
                if (mathematicAnswerObj != null)
                {
                    answerComputed = mathematicAnswerObj.ToString();
                }
            }
            catch
            {
                continue;
            }
            var newQuestion = new LearnQuestion
            {
                LearnSessionId = request.LearnSessionId,
                QuestionId = question.Id,
                Question = mathematicQuestion,
                QuestionType = QuestionType.Mathematic,
                Description = question.Digits.ToString(),
                RightAnswer = answerComputed ?? "",
                MarkedAsHard = hardQuestionIds.Contains(question.Id)
            };
            _dataContext.LearnQuestions.Add(newQuestion);
        }
        foreach (var question in set.QuestionMultipleChoices)
        {
            if (request.OnlyHardQuestions && !hardQuestionIds.Contains(question.Id))
            {
                continue;
            }
            var newQuestion = new LearnQuestion
            {
                LearnSessionId = request.LearnSessionId,
                QuestionId = question.Id,
                Question = question.Question,
                QuestionType = QuestionType.MultipleChoice,
                PossibleAnswers = string.Join("||", question.Answers.Select(ans => ans.Id.ToString() + "|" + ans.Answer)),
                RightAnswer = string.Join("||", question.Answers.Where(ans => ans.IsRight).Select(ans => ans.Id.ToString() + "|" + ans.Answer)),
                MarkedAsHard = hardQuestionIds.Contains(question.Id)
            };
            _dataContext.LearnQuestions.Add(newQuestion);
        }
        foreach (var question in set.QuestionOpenQuestions)
        {
            if (request.OnlyHardQuestions && !hardQuestionIds.Contains(question.Id))
            {
                continue;
            }
            var newQuestion = new LearnQuestion
            {
                LearnSessionId = request.LearnSessionId,
                QuestionId = question.Id,
                Question = question.Question,
                QuestionType = QuestionType.OpenQuestion,
                RightAnswer = question.Answer,
                MarkedAsHard = hardQuestionIds.Contains(question.Id)                
            };
            _dataContext.LearnQuestions.Add(newQuestion);
        }
        foreach (var question in set.QuestionTrueFalses)
        {
            if (request.OnlyHardQuestions && !hardQuestionIds.Contains(question.Id))
            {
                continue;
            }
            var newQuestion = new LearnQuestion
            {
                LearnSessionId = request.LearnSessionId,
                QuestionId = question.Id,
                Question = question.Question,
                QuestionType = QuestionType.TrueFalse,
                RightAnswer = question.Answer ? "true" : "false",
                MarkedAsHard = hardQuestionIds.Contains(question.Id)
            };
            _dataContext.LearnQuestions.Add(newQuestion);
        }
        foreach (var question in set.QuestionWords)
        {
            if (request.OnlyHardQuestions && !hardQuestionIds.Contains(question.Id))
            {
                continue;
            }
            var newQuestion = new LearnQuestion
            {
                LearnSessionId = request.LearnSessionId,
                QuestionId = question.Id,
                Question = question.LanguageSubjectMain,
                QuestionType = QuestionType.Word,
                Description = set.SubjectSecond == null ? null : ((int)set.SubjectSecond).ToString(),
                RightAnswer = question.LanguageSubjectSecond,
                MarkedAsHard = hardQuestionIds.Contains(question.Id)
            };
            _dataContext.LearnQuestions.Add(newQuestion);
        }
        await _dataContext.SaveChangesAsync();
        return Ok();
    }
}