﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Learnz.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class ChallengeAnswer : Controller
{
    private readonly DataContext _dataContext;
    private readonly IUserService _userService;
    private readonly IChallengeQueryService _challengeQueryService;
    public ChallengeAnswer(DataContext dataContext, IUserService userService, IChallengeQueryService challengeQueryService)
    {
        _dataContext = dataContext;
        _userService = userService;
        _challengeQueryService = challengeQueryService;
    }

    [HttpPost]
    public async Task<ActionResult> Answer(GeneralQuestionAnswerDTO request)
    {
        var guid = _userService.GetUserGuid();
        var challenge = await _dataContext.Challenges.Include(chl => chl.ChallengeQuestionAnswers).Include(chl => chl.ChallengeQuestionsPosed).FirstOrDefaultAsync(chl => chl.ChallengeUsers.Select(chu => chu.UserId).Contains(guid) && chl.Id == request.ChallengeId);
        if (challenge == null)
        {
            return BadRequest(ErrorKeys.ChallengeNotAccessible);
        }
        if (challenge.ChallengeQuestionAnswers.Any(ans => ans.UserId == guid && ans.ChallengeQuestionPosedId == request.QuestionId))
        {
            return BadRequest(ErrorKeys.ChallengeAlreadyAnswered);
        }
        var question = challenge.ChallengeQuestionsPosed.FirstOrDefault(qst => qst.QuestionId == request.QuestionId && qst.IsActive == true);
        if (question == null)
        {
            return BadRequest(ErrorKeys.ChallengeAnswerNotPossible);
        }

        if (question.Expires < DateTime.UtcNow)
        {
            var questionPosed = await _dataContext.ChallengeQuestiosnPosed.FirstOrDefaultAsync(qst => qst.QuestionId == request.QuestionId);
            if (questionPosed != null)
            {
                var challengeToChange = await _dataContext.Challenges.FirstAsync(chl => chl.Id == challenge.Id);
                challengeToChange.State = await _challengeQueryService.QuestionLeft(challenge.Id) ? ChallengeState.Result : ChallengeState.Ended;
                await _challengeQueryService.InactivateQuestions(challenge.Id);
                await _dataContext.SaveChangesAsync();
                await _challengeQueryService.TriggerWebsocketAllUsers(challenge.Id);
            }
            return BadRequest(ErrorKeys.ChallengeAnswerNotPossible);
        }
        
        bool isRight = await IsAnswerRight(question, request.Answer);
        double millisLeft = (question.Expires - DateTime.UtcNow).TotalMilliseconds;
        int points = isRight && millisLeft > 0 ? Convert.ToInt32(Math.Ceiling(millisLeft / 200)) : 0;
        var answer = new ChallengeQuestionAnswer
        {
            ChallengeId = challenge.Id,
            Answer = question.Answer.ToLower().Trim(),
            ChallengeQuestionPosedId = question.Id,
            IsRight = isRight,
            UserId = guid,
            Points = points
        };
        _dataContext.ChallengeQuestionAnswers.Add(answer);
        await _dataContext.SaveChangesAsync();

        await _challengeQueryService.TriggerWebsocket(challenge.Id, guid);

        int peopleAnswered = await _dataContext.ChallengeQuestionAnswers.Where(cqa => cqa.ChallengeQuestionPosedId == question.Id).CountAsync();
        int playerInGame = await _dataContext.ChallengeUsers.Where(chu => chu.ChallengeId == challenge.Id).CountAsync();
        if (peopleAnswered == playerInGame && challenge.State == ChallengeState.Question)
        {
            var challengeDb = await _dataContext.Challenges.FirstAsync(chl => chl.Id == challenge.Id);
            challengeDb.State = await _challengeQueryService.QuestionLeft(challenge.Id) ? ChallengeState.Result : ChallengeState.Ended;
            var questionPosed = challenge.ChallengeQuestionsPosed.FirstOrDefault(qst => qst.QuestionId == request.QuestionId && qst.IsActive == true);
            if (questionPosed != null)
            {
                await _challengeQueryService.InactivateQuestions(challenge.Id);
            }
            await _dataContext.SaveChangesAsync();
            await _challengeQueryService.TriggerWebsocketAllUsers(challenge.Id);
        }

        return Ok();
    }

    private async Task<bool> IsAnswerRight(ChallengeQuestionPosed question, string answer)
    {
        if (string.IsNullOrEmpty(question.Answer))
        {
            return false;
        }
        var posed = await _challengeQueryService.GetQuestionById(question.QuestionId);
        if (posed == null)
        {
            return false;
        }
        switch (posed.QuestionType)
        {
            case QuestionType.Distribute:
            case QuestionType.MultipleChoice:
                string delimiter = posed.QuestionType == QuestionType.Distribute ? "||" : "|";
                var distributeShould = question.Answer.ToLower().Split(delimiter).OrderBy(x => x);
                var distributeGive = answer.ToLower().Split(delimiter).OrderBy(x => x);
                if (distributeShould.Count() != distributeGive.Count())
                {
                    return false;
                }
                for (int i = 0; i < distributeShould.Count(); i++)
                {
                    if (distributeShould.ElementAt(i) != distributeGive.ElementAt(i))
                    {
                        return false;
                    }
                }
                return true;
            default:
                return question.Answer.ToLower().Trim() == answer.ToLower().Trim();
        }
    }
}
