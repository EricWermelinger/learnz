using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Learnz.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class ChallengeActive : Controller
{
    private readonly DataContext _dataContext;
    private readonly IUserService _userService;
    public ChallengeActive(DataContext dataContext, IUserService userService)
    {
        _dataContext = dataContext;
        _userService = userService;
    }

    [HttpGet]
    public async Task<ActionResult<ChallengeActiveDTO>> GetActiveChallenge(Guid challengeId)
    {
        var guid = _userService.GetUserGuid();
        var challenge = await _dataContext.Challenges.FirstOrDefaultAsync(chl => chl.Id == challengeId && (chl.ChallengeUsers.Select(chu => chu.UserId).Contains(guid) || chl.OwnerId == guid));
        if (challenge == null)
        {
            return BadRequest(ErrorKeys.ChallengeNotAccessible);
        }

        var currentQuestion = await _dataContext.ChallengeQuestiosnPosed.FirstOrDefaultAsync(cqp => cqp.Challenge.Id == challengeId
                                                                                                && cqp.Challenge.State != ChallengeState.Ended
                                                                                                && cqp.IsActive);

        var currentQuestionAnswered = await _dataContext.ChallengeQuestionAnswers.AnyAsync(cqa => currentQuestion != null && cqa.ChallengeQuestionPosedId == currentQuestion.Id && cqa.UserId == guid);
        var numberOfAnswers = await _dataContext.ChallengeQuestionAnswers.Where(cqa => currentQuestion != null && cqa.ChallengeQuestionPosedId == currentQuestion.Id).CountAsync();
        var numberOfPlayers = await _dataContext.ChallengeUsers.Where(chu => chu.ChallengeId == challengeId).CountAsync();
        var currentState = challenge.State == ChallengeState.BeforeGame || challenge.State == ChallengeState.Ended ? challenge.State : (
                (numberOfAnswers == numberOfPlayers && currentQuestion != null) ? ChallengeState.Result : (
                    currentQuestionAnswered ? ChallengeState.Answer : ChallengeState.Question
                )
            );
        
        bool isOwner = challenge.OwnerId == guid;
        var currentQuestionDto = currentState == ChallengeState.Question && currentQuestion != null ? await GetQuestionById(currentQuestion.QuestionId) : null;
        var result = await GetResult(challengeId);
        var lastQuestionPoints = !isOwner && currentState == ChallengeState.Result && currentQuestion != null ? await GetLastQuestionPoints(challengeId, currentQuestion.Id, guid) : null;

        var data = new ChallengeActiveDTO
        {
            Name = challenge.Name,
            Question = currentQuestionDto,
            IsOwner = isOwner,
            Result = result ?? new List<ChallengePlayerResultDTO>(),
            State = currentState,
            Cancelled = false,
            LastQuestionPoint = lastQuestionPoints,
        };
        return Ok(data);
    }

    private async Task<List<ChallengePlayerResultDTO>> GetResult(Guid challengeId)
    {
        var resultRows = await _dataContext.ChallengeQuestionAnswers
                                        .Where(qsa => qsa.ChallengeId == challengeId)
                                        .Select(qsa => new { Username = qsa.User.Username, UserId = qsa.UserId, Points = qsa.Points })
                                        .GroupBy(rsr => new { UserId = rsr.UserId, Username = rsr.Username })
                                        .Select(grp => new ChallengePlayerResultDTO { Username = grp.Key.Username, Points = grp.Sum(grp => grp.Points) })
                                        .ToListAsync();

        return resultRows ?? new List<ChallengePlayerResultDTO>();
    }

    private async Task<ChallengeQuestionDTO?> GetQuestionById(Guid questionId)
    {
        var questionDistribute = await _dataContext.CreateQuestionDistributes.Include(qst => qst.Answers).FirstOrDefaultAsync(qst => qst.Id == questionId);
        if (questionDistribute != null)
        {
            return new ChallengeQuestionDTO
            {
                Question = questionDistribute.Question,
                QuestionType = QuestionType.Distribute,
                AnswerSetOne = questionDistribute.Answers.Select(ans => new ChallengeQuestionAnswerDTO { AnswerId = ans.LeftSideId, Answer = ans.LeftSide }).Select(q => new { Question = q, Shuffle = Guid.NewGuid() }).OrderBy(q => q.Shuffle).Select(q => q.Question).ToList(),
                AnswerSetTwo = questionDistribute.Answers.Select(ans => new ChallengeQuestionAnswerDTO { AnswerId = ans.RightSideId, Answer = ans.RightSide }).Select(q => new { Question = q, Shuffle = Guid.NewGuid() }).OrderBy(q => q.Shuffle).Select(q => q.Question).ToList(),
            };
        }    

        var questionMathematic = await _dataContext.ChallengeQuestionsMathematicResolved.FirstOrDefaultAsync(qst => qst.Id == questionId);
        if (questionMathematic != null)
        {
            return new ChallengeQuestionDTO
            {
                Question = questionMathematic.Question,
                Description = questionMathematic.Digits.ToString(),
                QuestionType = QuestionType.Mathematic
            };
        }

        var questionMultipleChoice = await _dataContext.CreateQuestionMultipleChoices.Include(qst => qst.Answers).FirstOrDefaultAsync(qst => qst.Id == questionId);
        if (questionMultipleChoice != null)
        {
            return new ChallengeQuestionDTO
            {
                Question = questionMultipleChoice.Question,
                AnswerSetOne = questionMultipleChoice.Answers.Select(cqa => new ChallengeQuestionAnswerDTO { Answer = cqa.Answer, AnswerId = cqa.Id }).Select(q => new { Question = q, Shuffle = Guid.NewGuid() }).OrderBy(q => q.Shuffle).Select(q => q.Question).ToList(),
                QuestionType = QuestionType.MultipleChoice
            };
        }

        var openQuestion = await _dataContext.CreateQuestionOpenQuestions.FirstOrDefaultAsync(qst => qst.Id == questionId);
        if (openQuestion != null)
        {
            return new ChallengeQuestionDTO
            {
                Question = openQuestion.Question,
                QuestionType = QuestionType.OpenQuestion
            };
        }

        var questionText = await _dataContext.CreateQuestionTextFields.FirstOrDefaultAsync(qst => qst.Id == questionId);
        if (questionText != null)
        {
            return new ChallengeQuestionDTO
            {
                Question = questionText.Question,
                QuestionType = QuestionType.TextField
            };
        }
        
        var questionTrueFalse = await _dataContext.CreateQuestionTrueFalses.FirstOrDefaultAsync(qst => qst.Id == questionId);
        if (questionTrueFalse != null)
        {
            return new ChallengeQuestionDTO
            {
                Question = questionTrueFalse.Question,
                QuestionType = QuestionType.TrueFalse
            };
        }
        
        var questionWord = await _dataContext.CreateQuestionWords.FirstOrDefaultAsync(qst => qst.Id == questionId);
        if (questionWord != null)
        {
            return new ChallengeQuestionDTO
            {
                Question = questionWord.LanguageSubjectMain,
                Description = questionWord.LanguageSubjectSecond.ToString(),
                QuestionType = QuestionType.Word
            };
        }
        
        return null;
    }
    
    private async Task<int?> GetLastQuestionPoints(Guid challengeId, Guid questionId, Guid userId)
    {
        var lastQuestionPoins = await _dataContext.ChallengeQuestionAnswers.Where(cqa => cqa.UserId == userId && cqa.ChallengeId == challengeId && cqa.ChallengeQuestionPosedId == questionId).Select(cqa => cqa.Points).FirstOrDefaultAsync();
        return lastQuestionPoins;
    }
}
