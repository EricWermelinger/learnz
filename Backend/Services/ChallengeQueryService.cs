using Learnz.Controllers;
using Microsoft.AspNetCore.SignalR;

namespace Learnz.Services;
public class ChallengeQueryService : IChallengeQueryService
{
    private readonly DataContext _dataContext;
    private readonly HubService _hubService;
    public ChallengeQueryService(DataContext dataContext, IHubContext<LearnzHub> learnzHub)
    {
        _dataContext = dataContext;
        _hubService = new HubService(learnzHub);
    }

    public async Task TriggerWebsocket(Guid challengeId, Guid guid)
    {
        var challenge = await _dataContext.Challenges.FirstOrDefaultAsync(chl => chl.Id == challengeId && (chl.ChallengeUsers.Select(chu => chu.UserId).Contains(guid) || chl.OwnerId == guid));
        if (challenge != null)
        {
            var data = await GetActiveChallenge(challenge, guid);
            await TriggerWebsocket(data, challengeId, guid);
        }
    }

    public async Task TriggerWebsocket(ChallengeActiveDTO challenge, Guid challengeId, Guid guid)
    {
        await _hubService.SendMessageToUser(nameof(ChallengeActive), challenge, guid, challengeId);
    }

    public async Task TriggerWebsocketAllUsers(Guid challengeId)
    {
        var userIds = await _dataContext.ChallengeUsers.Where(chu => chu.ChallengeId == challengeId).Select(chu => chu.UserId).ToListAsync();
        var ownerId = await _dataContext.Challenges.Where(chl => chl.Id == challengeId).Select(chl => chl.OwnerId).FirstAsync();
        var challengeActive = await GetActiveChallenge(challengeId);
        foreach (Guid userId in userIds)
        {
            await TriggerWebsocket(challengeActive, challengeId, userId);
        }
        await TriggerWebsocket(challengeActive, challengeId, ownerId);
    }

    public async Task<ChallengeActiveDTO> GetActiveChallenge(Guid challengeId)
    {
        var challenge = await _dataContext.Challenges.FirstAsync(chl => chl.Id == challengeId);
        var active = await GetActiveChallenge(challenge, null);
        return active;
    }

    public async Task<ChallengeActiveDTO> GetActiveChallenge(Challenge challenge, Guid? guid)
    {
        Guid challengeId = challenge.Id;
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

        bool isOwner = guid == null ? false : challenge.OwnerId == guid;
        var currentQuestionDto = currentState == ChallengeState.Question && currentQuestion != null ? await GetQuestionById(currentQuestion.QuestionId) : null;
        var result = await GetResult(challengeId);
        var lastQuestionPoints = guid != null && !isOwner && currentState == ChallengeState.Result && currentQuestion != null ? await GetLastQuestionPoints(challengeId, currentQuestion.Id, (Guid)guid) : null;

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
        return data;
    }

    public async Task<List<ChallengePlayerResultDTO>> GetResult(Guid challengeId)
    {
        var users = await _dataContext.ChallengeUsers.Where(chu => chu.ChallengeId == challengeId).Select(chu => new { UserId = chu.UserId, Username = chu.User.Username }).ToListAsync();
        if (users == null)
        {
            return new List<ChallengePlayerResultDTO>();
        }
        var resultRows = await _dataContext.ChallengeQuestionAnswers
                                        .Where(qsa => qsa.ChallengeId == challengeId)
                                        .Select(qsa => new { Username = qsa.User.Username, UserId = qsa.UserId, Points = qsa.Points })
                                        .GroupBy(rsr => new { UserId = rsr.UserId, Username = rsr.Username })
                                        .Select(grp => new { UserId = grp.Key.UserId, Username = grp.Key.Username, Points = grp.Sum(grp => grp.Points) })
                                        .ToListAsync();
        var result = new List<ChallengePlayerResultDTO>();
        foreach (var user in users)
        {
            var correspondingRow = resultRows.FirstOrDefault(r => r.UserId == user.UserId);
            var resultUser = new ChallengePlayerResultDTO
            {
                Username = user.Username,
                Points = correspondingRow == null ? 0 : correspondingRow.Points
            };
            result.Add(resultUser);
        }
        return result.OrderByDescending(r => r.Points).ToList();
    }

    public async Task<GeneralQuestionQuestionDTO?> GetQuestionById(Guid questionId)
    {
        var questionDistribute = await _dataContext.CreateQuestionDistributes.Include(qst => qst.Answers).FirstOrDefaultAsync(qst => qst.Id == questionId);
        if (questionDistribute != null)
        {
            return new GeneralQuestionQuestionDTO
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
            return new GeneralQuestionQuestionDTO
            {
                Question = questionMathematic.Question,
                Description = questionMathematic.Digits.ToString(),
                QuestionType = QuestionType.Mathematic
            };
        }

        var questionMultipleChoice = await _dataContext.CreateQuestionMultipleChoices.Include(qst => qst.Answers).FirstOrDefaultAsync(qst => qst.Id == questionId);
        if (questionMultipleChoice != null)
        {
            return new GeneralQuestionQuestionDTO
            {
                Question = questionMultipleChoice.Question,
                AnswerSetOne = questionMultipleChoice.Answers.Select(cqa => new ChallengeQuestionAnswerDTO { Answer = cqa.Answer, AnswerId = cqa.Id }).Select(q => new { Question = q, Shuffle = Guid.NewGuid() }).OrderBy(q => q.Shuffle).Select(q => q.Question).ToList(),
                QuestionType = QuestionType.MultipleChoice
            };
        }

        var openQuestion = await _dataContext.CreateQuestionOpenQuestions.FirstOrDefaultAsync(qst => qst.Id == questionId);
        if (openQuestion != null)
        {
            return new GeneralQuestionQuestionDTO
            {
                Question = openQuestion.Question,
                QuestionType = QuestionType.OpenQuestion
            };
        }

        var questionTrueFalse = await _dataContext.CreateQuestionTrueFalses.FirstOrDefaultAsync(qst => qst.Id == questionId);
        if (questionTrueFalse != null)
        {
            return new GeneralQuestionQuestionDTO
            {
                Question = questionTrueFalse.Question,
                QuestionType = QuestionType.TrueFalse
            };
        }

        var questionWord = await _dataContext.CreateQuestionWords.FirstOrDefaultAsync(qst => qst.Id == questionId);
        if (questionWord != null)
        {
            return new GeneralQuestionQuestionDTO
            {
                Question = questionWord.LanguageSubjectMain,
                Description = questionWord.LanguageSubjectSecond.ToString(),
                QuestionType = QuestionType.Word
            };
        }

        return null;
    }

    public async Task<int?> GetLastQuestionPoints(Guid challengeId, Guid questionId, Guid userId)
    {
        var lastQuestionPoins = await _dataContext.ChallengeQuestionAnswers.Where(cqa => cqa.UserId == userId && cqa.ChallengeId == challengeId && cqa.ChallengeQuestionPosedId == questionId).Select(cqa => cqa.Points).FirstOrDefaultAsync();
        return lastQuestionPoins;
    }
}