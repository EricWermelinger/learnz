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

    public async Task TriggerWebsocketAllUsers(Guid challengeId, bool? cancelledEvluated = null)
    {
        var userIds = await _dataContext.ChallengeUsers.Where(chu => chu.ChallengeId == challengeId).Select(chu => chu.UserId).ToListAsync();
        var ownerId = await _dataContext.Challenges.Where(chl => chl.Id == challengeId).Select(chl => chl.OwnerId).FirstAsync();
        var challenge = await _dataContext.Challenges.FirstAsync(chl => chl.Id == challengeId);
        var currentQuestionEvaluated = await GetCurrentQuestion(challengeId);
        int numberOfAnswersEvaluated = await GetNumberOfAnswers(currentQuestionEvaluated);
        int numberOfPlayersEvaluated = await GetNumberOfPlayers(challengeId);
        var currentQuestionDtoEvaluated = currentQuestionEvaluated == null ? null : await GetQuestionById(currentQuestionEvaluated.QuestionId);
        var resultEvaluated = await GetResult(challengeId);
        var lastQuestionCorrectAnswerEvaluated = challenge.State == ChallengeState.Result ? await GetLastQuestionCorrectAnswer(challengeId) : null;
        foreach (Guid userId in userIds)
        {
            var challengeActive = await GetActiveChallenge(challenge, userId, currentQuestionEvaluated, numberOfAnswersEvaluated, numberOfPlayersEvaluated, currentQuestionDtoEvaluated, resultEvaluated, cancelledEvluated, lastQuestionCorrectAnswerEvaluated);
            await TriggerWebsocket(challengeActive, challengeId, userId);
        }
        var challengeActiveOwner = await GetActiveChallenge(challenge, ownerId, currentQuestionEvaluated, numberOfAnswersEvaluated, numberOfPlayersEvaluated, currentQuestionDtoEvaluated, resultEvaluated, cancelledEvluated, lastQuestionCorrectAnswerEvaluated);
        await TriggerWebsocket(challengeActiveOwner, challengeId, ownerId);
    }

    public async Task InactivateQuestions(Guid challengeId)
    {
        var questions = await _dataContext.ChallengeQuestiosnPosed.Where(cqp => cqp.ChallengeId == challengeId && cqp.IsActive).ToListAsync();
        foreach (ChallengeQuestionPosed posed in questions)
        {
            posed.IsActive = false;
        }
        await _dataContext.SaveChangesAsync();
    }

    public async Task<bool> QuestionLeft(Guid challengeId)
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

        return questionsLeftDistributesIds.Count > 0
            || questionsLeftMathematicIds.Count > 0
            || questionsLeftMultipleChoicesIds.Count > 0
            || questionsLeftOpenQuestionsIds.Count > 0
            || questionsLeftTrueFalseIds.Count > 0
            || questionsLeftWordIds.Count > 0;
    }

    private async Task<ChallengeQuestionPosed?> GetCurrentQuestion(Guid challengeId)
    {
        var currentQuestion = await _dataContext.ChallengeQuestiosnPosed
            .Where(cqp => cqp.Challenge.Id == challengeId && cqp.Challenge.State != ChallengeState.Ended)
            .OrderByDescending(cqp => cqp.Created)
            .FirstOrDefaultAsync();
        return currentQuestion;
    }

    private async Task<int> GetNumberOfAnswers(ChallengeQuestionPosed? currentQuestion)
    {
        if (currentQuestion == null)
        {
            return 0;
        }
        var numberOfAnswers = await _dataContext.ChallengeQuestionAnswers.Where(cqa => currentQuestion != null && cqa.ChallengeQuestionPosedId == currentQuestion.Id).CountAsync();
        return numberOfAnswers;
    }

    private async Task<int> GetNumberOfPlayers(Guid challengeId)
    {
        var numberOfPlayers = await _dataContext.ChallengeUsers.Where(chu => chu.ChallengeId == challengeId).CountAsync();
        return numberOfPlayers;
    }

    public async Task<ChallengeActiveDTO> GetActiveChallenge(Challenge challenge, Guid guid,
        ChallengeQuestionPosed? currentQuestionEvaluated = null, int? numberOfAnswersEvaluated = null,
        int? numberOfPlayersEvaluated = null, GeneralQuestionQuestionDTO? currentQuestionDtoEvaluated = null,
        List<ChallengePlayerResultDTO>? resultEvaluated = null, bool? cancelledEvluated = null, string? lastQuestionCorrectAnswerEvaluated = null)
    {
        Guid challengeId = challenge.Id;
        var currentQuestion = currentQuestionEvaluated ?? await GetCurrentQuestion(challengeId);
        var currentQuestionAnswered = await _dataContext.ChallengeQuestionAnswers.AnyAsync(cqa => currentQuestion != null && cqa.ChallengeQuestionPosedId == currentQuestion.Id && cqa.UserId == guid);
        var numberOfAnswers = numberOfAnswersEvaluated ?? await GetNumberOfAnswers(currentQuestion);
        var numberOfPlayers = numberOfPlayersEvaluated ?? await GetNumberOfPlayers(challengeId);
        var currentState = challenge.State == ChallengeState.BeforeGame || challenge.State == ChallengeState.Ended || challenge.State == ChallengeState.Result ? challenge.State : (
            currentQuestionAnswered ? ChallengeState.Answer : ChallengeState.Question);

        bool isOwner = challenge.OwnerId == guid;
        var currentQuestionDto = currentState == ChallengeState.Question && currentQuestion != null ? (currentQuestionDtoEvaluated ?? await GetQuestionById(currentQuestion.QuestionId)) : null;
        var result = resultEvaluated ?? await GetResult(challengeId);
        var lastQuestionPoints = !isOwner && currentState == ChallengeState.Result && currentQuestion != null ? await GetLastQuestionPoints(challengeId, currentQuestion.Id, (Guid)guid) : null;
        var lastQuestionCorrectAnswer = currentState == ChallengeState.Result ? lastQuestionCorrectAnswerEvaluated ?? await GetLastQuestionCorrectAnswer(challengeId) : null;
        var questionCloses = currentQuestion?.Expires;

        var data = new ChallengeActiveDTO
        {
            Name = challenge.Name,
            Question = currentQuestionDto,
            IsOwner = isOwner,
            Result = result ?? new List<ChallengePlayerResultDTO>(),
            State = currentState,
            Cancelled = cancelledEvluated ?? false,
            LastQuestionPoint = lastQuestionPoints,
            LastQuestionCorrectAnswer = lastQuestionCorrectAnswer,
            QuestionCloses = questionCloses
        };
        return data;
    }

    private async Task<string?> GetLastQuestionCorrectAnswer(Guid challengeId)
    {
        var lastQuestion = await _dataContext.ChallengeQuestiosnPosed.Where(qsp => qsp.ChallengeId == challengeId)
            .OrderByDescending(qsp => qsp.Created)
            .FirstOrDefaultAsync();
        if (lastQuestion == null)
        {
            return null;
        }
        var lastQuestionDistribute = await _dataContext.CreateQuestionDistributes.Include(qst => qst.Answers).FirstOrDefaultAsync(qst => qst.Id == lastQuestion.QuestionId);
        if (lastQuestionDistribute != null)
        {
            return string.Join(" & ", lastQuestionDistribute.Answers.Select(ans => ans.LeftSide + " - " + ans.RightSide));
        }
        var lastQuestionMultipleChoice = await _dataContext.CreateQuestionMultipleChoices.Include(qst => qst.Answers).FirstOrDefaultAsync(qst => qst.Id == lastQuestion.QuestionId);
        if (lastQuestionMultipleChoice != null)
        {
            return string.Join(" & ", lastQuestionMultipleChoice.Answers.Where(ans => ans.IsRight).Select(ans => ans.Answer));
        }
        return lastQuestion.Answer;
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
                QuestionId = questionDistribute.Id,
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
                QuestionId = questionMathematic.Id,
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
                QuestionId = questionMultipleChoice.Id,
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
                QuestionId = openQuestion.Id,
                Question = openQuestion.Question,
                QuestionType = QuestionType.OpenQuestion
            };
        }

        var questionTrueFalse = await _dataContext.CreateQuestionTrueFalses.FirstOrDefaultAsync(qst => qst.Id == questionId);
        if (questionTrueFalse != null)
        {
            return new GeneralQuestionQuestionDTO
            {
                QuestionId = questionTrueFalse.Id,
                Question = questionTrueFalse.Question,
                QuestionType = QuestionType.TrueFalse
            };
        }

        var questionWord = await _dataContext.CreateQuestionWords.FirstOrDefaultAsync(qst => qst.Id == questionId);
        if (questionWord != null)
        {
            var secondSubject = await _dataContext.CreateSets.Where(crs => crs.Id == questionWord.SetId).Select(crs => crs.SubjectSecond).FirstOrDefaultAsync();
            return new GeneralQuestionQuestionDTO
            {
                QuestionId = questionWord.Id,
                Question = questionWord.LanguageSubjectMain,
                Description = secondSubject == null ? null : ((int)secondSubject).ToString(),
                QuestionType = QuestionType.Word
            };
        }

        return null;
    }

    public async Task<int?> GetLastQuestionPoints(Guid challengeId, Guid questionId, Guid userId)
    {
        var lastQuestionPoints = await _dataContext.ChallengeQuestionAnswers.Where(cqa => cqa.UserId == userId && cqa.ChallengeId == challengeId && cqa.ChallengeQuestionPosedId == questionId).Select(cqa => cqa.Points).FirstOrDefaultAsync();
        return lastQuestionPoints;
    }
}