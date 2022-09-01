using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace Learnz.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class ChallengeFlow : Controller
{
    private readonly DataContext _dataContext;
    private readonly IUserService _userService;
    private readonly IChallengeQueryService _challengeQueryService;
    public ChallengeFlow(DataContext dataContext, IUserService userService, IChallengeQueryService challengeQueryService)
    {
        _dataContext = dataContext;
        _userService = userService;
        _challengeQueryService = challengeQueryService;
    }

    [HttpPost]
    public async Task<ActionResult> NextFlow(ChallengeIdDTO request)
    {
        var guid = _userService.GetUserGuid();
        var challenge = await _dataContext.Challenges
                                          .Include(chl => chl.CreateSet)
                                          .FirstOrDefaultAsync(chl => chl.State != ChallengeState.Ended && chl.OwnerId == guid && chl.Id == request.ChallengeId);
        if (challenge == null)
        {
            return BadRequest(ErrorKeys.ChallengeNotAccessible);
        }
                                    
        switch (challenge.State)
        {
            case ChallengeState.BeforeGame:
                await PoseQuestion(challenge.Id);
                challenge.State = ChallengeState.Question;
                await _dataContext.SaveChangesAsync();
                await _challengeQueryService.TriggerWebsocketAllUsers(challenge.Id);
                break;
            case ChallengeState.Question:
                await _challengeQueryService.InactivateQuestions(challenge.Id);
                bool questionLeft = await _challengeQueryService.QuestionLeft(challenge.Id);
                challenge.State = questionLeft ? ChallengeState.Result : ChallengeState.Ended;
                await _dataContext.SaveChangesAsync();
                await _challengeQueryService.TriggerWebsocketAllUsers(challenge.Id);
                break;
            case ChallengeState.Result:
                await PoseQuestion(challenge.Id);
                challenge.State = ChallengeState.Question;
                await _dataContext.SaveChangesAsync();
                await _challengeQueryService.TriggerWebsocketAllUsers(challenge.Id);
                break;
            default:
                break;
           
        }
        return Ok();
    }

    private async Task PoseQuestion(Guid challengeId)
    {
        var questionPosedIds = await _dataContext.ChallengeQuestiosnPosed.Where(cqp => cqp.ChallengeId == challengeId).Select(cqp => cqp.QuestionId).ToListAsync();
        var questionMathematicPosedIds = await _dataContext.ChallengeQuestionsMathematicResolved.Where(cqp => cqp.ChallengeId == challengeId).Select(cqp => cqp.QuestionMathematicId).ToListAsync();
        var setId = await _dataContext.Challenges.Where(chl => chl.Id == challengeId).Select(chl => chl.CreateSetId).FirstAsync();
        var set = await _dataContext.CreateSets
            .Include(crs => crs.QuestionDistributes)
            .ThenInclude(qd => qd.Answers)
            .Include(crs => crs.QuestionMathematics)
            .ThenInclude(qm => qm.Variables)
            .Include(crs => crs.QuestionMultipleChoices)
            .ThenInclude(qmc => qmc.Answers)
            .Include(crs => crs.QuestionOpenQuestions)
            .Include(crs => crs.QuestionTrueFalses)
            .Include(crs => crs.QuestionWords)
            .FirstAsync(crs => crs.Id == setId);
        
        var questionsLeftDistributesIds = set.QuestionDistributes.Where(qst => !questionPosedIds.Contains(qst.Id)).ToList();
        var questionsLeftMathematicIds = set.QuestionMathematics.Where(qst => !questionMathematicPosedIds.Contains(qst.Id)).ToList();
        var questionsLeftMultipleChoicesIds = set.QuestionMultipleChoices.Where(qst => !questionPosedIds.Contains(qst.Id)).ToList();
        var questionsLeftOpenQuestionsIds = set.QuestionOpenQuestions.Where(qst => !questionPosedIds.Contains(qst.Id)).ToList();
        var questionsLeftTrueFalseIds = set.QuestionTrueFalses.Where(qst => !questionPosedIds.Contains(qst.Id)).ToList();
        var questionsLeftWordIds = set.QuestionWords.Where(qst => !questionPosedIds.Contains(qst.Id)).ToList();

        var questionsLeft = new List<Guid>();
        if (questionsLeftDistributesIds != null)
        {
            foreach (var questionLeftDistribute in questionsLeftDistributesIds)
            {
                questionsLeft.Add(questionLeftDistribute.Id);
            }
        }
        if (questionsLeftMathematicIds != null)
        {
            foreach (var questionLeftMathematic in questionsLeftMathematicIds)
            {
                questionsLeft.Add(questionLeftMathematic.Id);
            }
        }
        if (questionsLeftMultipleChoicesIds != null)
        {
            foreach (var questionLeftMultipleChoices in questionsLeftMultipleChoicesIds)
            {
                questionsLeft.Add(questionLeftMultipleChoices.Id);
            }
        }
        if (questionsLeftOpenQuestionsIds != null)
        {
            foreach (var questionLeftOpenQuestions in questionsLeftOpenQuestionsIds)
            {
                questionsLeft.Add(questionLeftOpenQuestions.Id);
            }
        }
        if (questionsLeftTrueFalseIds != null)
        {
            foreach (var questionLeftTrueFalse in questionsLeftTrueFalseIds)
            {
                questionsLeft.Add(questionLeftTrueFalse.Id);
            }
        }
        if (questionsLeftWordIds != null)
        {
            foreach (var questionLeftWord in questionsLeftWordIds)
            {
                questionsLeft.Add(questionLeftWord.Id);
            }
        }

        var nextQuestionId = questionsLeft.Select(q => new { Question = q, Random = Guid.NewGuid() }).OrderBy(q => q.Random).Select(q => q.Question).First();
        DateTime timestamp = DateTime.UtcNow;
        var nextQuestion = new ChallengeQuestionPosed
        {
            QuestionId = nextQuestionId,
            IsActive = true,
            Created = timestamp,
            Expires = timestamp.AddSeconds(20),
            ChallengeId = challengeId
        };
        if (questionsLeftMathematicIds != null && questionsLeftMathematicIds.Select(qst => qst.Id).Contains(nextQuestionId))
        {
            var nextQuestionMathematicId = await ResolveMathematicQuestion(challengeId, nextQuestionId);
            if (nextQuestionMathematicId != null)
            {
                var questionMathematicResolved = await _dataContext.ChallengeQuestionsMathematicResolved.FirstAsync(qst => nextQuestionMathematicId != null && qst.Id == nextQuestionMathematicId);
                nextQuestion.QuestionId = questionMathematicResolved.Id;
                nextQuestion.Answer = questionMathematicResolved.Answer.ToString();
                _dataContext.ChallengeQuestiosnPosed.Add(nextQuestion);
                await _dataContext.SaveChangesAsync();
                return;
            }
        }
        var nextQuestionDto = await _challengeQueryService.GetQuestionById(nextQuestionId);
        if (nextQuestionDto != null)
        {
            switch (nextQuestionDto.QuestionType)
            {
                case QuestionType.Distribute:
                    var newQuestionDistribute = await _dataContext.CreateQuestionDistributes.FirstAsync(qst => qst.Id == nextQuestionId);
                    var newQuestionDistributeAnswers = await _dataContext.CreateQuestionDistributeAnswers.Where(ans => ans.QuestionDistributeId == newQuestionDistribute.Id).Select(ans => ans.LeftSideId.ToString() + "|" + ans.RightSideId.ToString()).ToListAsync();
                    nextQuestion.Answer = string.Join("||", newQuestionDistributeAnswers);
                    break;
                case QuestionType.Mathematic:
                    break;
                case QuestionType.MultipleChoice:
                    var newQuestionMultipleChoice = await _dataContext.CreateQuestionMultipleChoices.FirstAsync(qst => qst.Id == nextQuestionId);
                    var newQuestionMutlipleChoiceAnswers = await _dataContext.CreateQuestionMultipleChoiceAnswers.Where(ans => ans.IsRight && ans.QuestionMultipleChoiceId == newQuestionMultipleChoice.Id).Select(ans => ans.Id.ToString()).ToListAsync();
                    nextQuestion.Answer = string.Join("|", newQuestionMutlipleChoiceAnswers);
                    break;
                case QuestionType.OpenQuestion:
                    var newQuestionOpen = await _dataContext.CreateQuestionOpenQuestions.FirstAsync(qst => qst.Id == nextQuestionId);
                    nextQuestion.Answer = newQuestionOpen.Answer;
                    break;
                case QuestionType.TrueFalse:
                    var newQuestionTrueFalse = await _dataContext.CreateQuestionTrueFalses.FirstAsync(qst => qst.Id == nextQuestionId);
                    nextQuestion.Answer = newQuestionTrueFalse.Answer.ToString();
                    break;
                case QuestionType.Word:
                    var newQuestionWord = await _dataContext.CreateQuestionWords.FirstAsync(qst => qst.Id == nextQuestionId);
                    nextQuestion.Answer = newQuestionWord.LanguageSubjectSecond;
                    break;
            }
            _dataContext.ChallengeQuestiosnPosed.Add(nextQuestion);
            await _dataContext.SaveChangesAsync();
        }
    }

    private async Task<Guid?> ResolveMathematicQuestion(Guid challengeId, Guid nextQuestionId)
    {
        if (nextQuestionId == Guid.Empty)
        {
            return null;
        }

        var nextId = Guid.NewGuid();
        var newQuestionMathematic = await _dataContext.CreateQuestionMathematics.FirstAsync(qst => qst.Id == nextQuestionId);
        var newQuestionVariables = await _dataContext.CreateQuestionMathematicVariables.Where(vrb => vrb.QuestionMathematicId == newQuestionMathematic.Id).ToListAsync();
        string mathematicQuestion = newQuestionMathematic.Question;
        string mathematicAnswer = newQuestionMathematic.Answer;
        Random random = new();
        foreach (var variable in newQuestionVariables)
        {
            if (variable != null)
            {
                double stepsDouble = (double)(variable.Max - variable.Min) / (double)(variable.Interval == 0 ? 1 : variable.Interval);
                int steps = (int)Math.Floor(stepsDouble);
                int randomStep = random.Next(0, steps);
                double variableValue = Math.Round(variable.Min + randomStep * (variable.Interval == 0 ? 1 : variable.Interval), variable.Digits);
                mathematicQuestion = mathematicQuestion.Replace(variable.Display, variableValue.ToString());
                mathematicAnswer = mathematicAnswer.Replace(variable.Display, variableValue.ToString());
            }
        }
        string mathematicAnswerComputed = ComputeMathematic(mathematicAnswer);
        var questionMathematicResolved = new ChallengeQuestionMathematicResolved
        {
            Id = nextId,
            ChallengeId = challengeId,
            Digits = newQuestionMathematic.Digits,
            Question = mathematicQuestion,
            Answer = Convert.ToDouble(mathematicAnswerComputed),
            QuestionMathematicId = nextQuestionId,
        };
        _dataContext.ChallengeQuestionsMathematicResolved.Add(questionMathematicResolved);
        await _dataContext.SaveChangesAsync();
        return nextId;
    }

    private string ComputeMathematic(string answer)
    {
        DataTable dt = new DataTable();
        try
        {
            var mathematicAnswerObj = dt.Compute(answer, "");
            if (mathematicAnswerObj != null)
            {
                return mathematicAnswerObj.ToString();
            }
        }
        catch { }
        return "";
    }
}
