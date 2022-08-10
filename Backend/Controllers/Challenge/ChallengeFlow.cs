using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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

        var set = await _dataContext.CreateSets
                                    .Where(crs => crs.Id == challenge.CreateSetId)
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
                                    
        switch (challenge.State)
        {
            case ChallengeState.BeforeGame:
                await PoseQuestion(challenge.Id);
                await _challengeQueryService.TriggerWebsocketAllUsers(challenge.Id);
                challenge.State = ChallengeState.Question;
                break;
            case ChallengeState.Question:
                await InactivateQuestions(challenge.Id);
                bool questionLeft = await QuestionLeft(challenge.Id);
                challenge.State = questionLeft ? ChallengeState.Result : ChallengeState.Ended;
                await _challengeQueryService.TriggerWebsocketAllUsers(challenge.Id);
                break;
            case ChallengeState.Result:
                await PoseQuestion(challenge.Id);
                await _challengeQueryService.TriggerWebsocketAllUsers(challenge.Id);
                challenge.State = ChallengeState.Question;
                break;
            default:
                break;
           
        }
        return Ok();
    }

    private async Task<bool> QuestionLeft(Guid challengeId)
    {
        var questionPosedIds = await _dataContext.ChallengeQuestiosnPosed.Where(cqp => cqp.ChallengeId == challengeId).Select(cqp => cqp.QuestionId).ToListAsync();
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
        var questionsLeftMathematicIds = set.QuestionMathematics.Where(qst => !questionPosedIds.Contains(qst.Id)).ToList();
        var questionsLeftMultipleChoicesIds = set.QuestionMultipleChoices.Where(qst => !questionPosedIds.Contains(qst.Id)).ToList();
        var questionsLeftOpenQuestionsIds = set.QuestionOpenQuestions.Where(qst => !questionPosedIds.Contains(qst.Id)).ToList();
        var questionsLeftTrueFalseIds = set.QuestionTrueFalses.Where(qst => !questionPosedIds.Contains(qst.Id)).ToList();
        var questionsLeftWordIds = set.QuestionWords.Where(qst => !questionPosedIds.Contains(qst.Id)).ToList();

        return questionsLeftDistributesIds.Count > 0
            || questionsLeftMathematicIds.Count > 0
            || questionsLeftMultipleChoicesIds.Count > 0
            || questionsLeftOpenQuestionsIds.Count > 0
            || questionsLeftTrueFalseIds.Count > 0
            || questionsLeftWordIds.Count > 0;
    }

    private async Task InactivateQuestions(Guid challengeId)
    {
        var questions = await _dataContext.ChallengeQuestiosnPosed.Where(cqp => cqp.ChallengeId == challengeId && cqp.IsActive).ToListAsync();
        foreach (ChallengeQuestionPosed posed in questions)
        {
            posed.IsActive = false;
        }
        await _dataContext.SaveChangesAsync();
    }

    private async Task PoseQuestion(Guid challengeId)
    {
        var questionPosedIds = await _dataContext.ChallengeQuestiosnPosed.Where(cqp => cqp.ChallengeId == challengeId).Select(cqp => cqp.QuestionId).ToListAsync();
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
        var questionsLeftMathematicIds = set.QuestionMathematics.Where(qst => !questionPosedIds.Contains(qst.Id)).ToList();
        var questionsLeftMultipleChoicesIds = set.QuestionMultipleChoices.Where(qst => !questionPosedIds.Contains(qst.Id)).ToList();
        var questionsLeftOpenQuestionsIds = set.QuestionOpenQuestions.Where(qst => !questionPosedIds.Contains(qst.Id)).ToList();
        var questionsLeftTrueFalseIds = set.QuestionTrueFalses.Where(qst => !questionPosedIds.Contains(qst.Id)).ToList();
        var questionsLeftWordIds = set.QuestionWords.Where(qst => !questionPosedIds.Contains(qst.Id)).ToList();

        var questionsLeft = new List<Guid>();
        if (questionsLeftDistributesIds != null)
        {
            questionsLeft.AddRange((IEnumerable<Guid>)questionsLeftDistributesIds);
        }
        if (questionsLeftMathematicIds != null)
        {
            questionsLeft.AddRange((IEnumerable<Guid>)questionsLeftMathematicIds);
        }
        if (questionsLeftMultipleChoicesIds != null)
        {
            questionsLeft.AddRange((IEnumerable<Guid>)questionsLeftMultipleChoicesIds);
        }
        if (questionsLeftOpenQuestionsIds != null)
        {
            questionsLeft.AddRange((IEnumerable<Guid>)questionsLeftOpenQuestionsIds);
        }
        if (questionsLeftTrueFalseIds != null)
        {
            questionsLeft.AddRange((IEnumerable<Guid>)questionsLeftTrueFalseIds);
        }
        if (questionsLeftWordIds != null)
        {
            questionsLeft.AddRange((IEnumerable<Guid>)questionsLeftWordIds);
        }

        var nextQuestionId = questionsLeft.Select(q => new { Question = q, Random = Guid.NewGuid() }).OrderBy(q => q.Random).Select(q => q.Question).First();
        var nextQuestionDto = await _challengeQueryService.GetQuestionById(nextQuestionId);
        if (nextQuestionDto != null)
        {
            DateTime timestamp = DateTime.UtcNow;
            var nextQuestion = new ChallengeQuestionPosed
            {
                QuestionId = nextQuestionId,
                IsActive = true,
                Created = timestamp,
                Expires = timestamp.AddSeconds(20),
                ChallengeId = challengeId
            };

            switch (nextQuestionDto.QuestionType)
            {
                case QuestionType.Distribute:
                    var newQuestionDistribute = await _dataContext.CreateQuestionDistributes.FirstAsync(qst => qst.Id == nextQuestionId);
                    var newQuestionDistributeAnswers = await _dataContext.CreateQuestionDistributeAnswers.Where(ans => ans.QuestionDistributeId == newQuestionDistribute.Id).Select(ans => ans.LeftSide + "|" + ans.RightSide).ToListAsync();
                    nextQuestion.Answer = string.Join("||", newQuestionDistributeAnswers);
                    break;
                case QuestionType.Mathematic:
                    var newQuestionMathematic = await _dataContext.CreateQuestionMathematics.FirstAsync(qst => qst.Id == nextQuestionId);
                    // todo insert resolved
                    break;
                case QuestionType.MultipleChoice:
                    var newQuestionMultipleChoice = await _dataContext.CreateQuestionMultipleChoices.FirstAsync(qst => qst.Id == nextQuestionId);
                    var newQuestionMutlipleChoiceAnswers = await _dataContext.CreateQuestionMultipleChoiceAnswers.Where(ans => ans.IsRight && ans.QuestionMultipleChoiceId == newQuestionMultipleChoice.Id).Select(ans => ans.Answer).ToListAsync();
                    nextQuestion.Answer = string.Join('|', newQuestionMutlipleChoiceAnswers);
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
        }
    }
}
