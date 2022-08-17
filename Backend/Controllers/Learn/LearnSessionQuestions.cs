﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Learnz.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class LearnSessionQuestions : Controller
{
    private readonly DataContext _dataContext;
    private readonly IUserService _userService;
    public LearnSessionQuestions(DataContext dataContext, IUserService userService)
    {
        _dataContext = dataContext;
        _userService = userService;
    }

    [HttpGet]
    public async Task<ActionResult<List<LearnQuestionDTO>>> GetQuestions(Guid learnSessionId)
    {
        var guid = _userService.GetUserGuid();
        var session = await _dataContext.LearnSessions.Where(lss => lss.UserId == guid && lss.Id == learnSessionId).Include(lss => lss.Questions).FirstOrDefaultAsync();
        if (session == null)
        {
            return BadRequest(ErrorKeys.LearnSessionNotAccessible);
        }
        var questions = session.Questions.Select(lqs => new LearnQuestionDTO
        {
            Answered = lqs.AnswerByUser != null,
            AnsweredCorrect = lqs.AnsweredCorrect,
            MarkedAsHard = lqs.MarkedAsHard ?? false,
            Question = new GeneralQuestionQuestionDTO
            {
                QuestionId = lqs.QuestionId,
                Question = lqs.Question,
                Description = lqs.Description,
                QuestionType = lqs.QuestionType,
                AnswerSetOne = lqs.PossibleAnswers == null ? null : GetAnswerSet(lqs, true),
                AnswerSetTwo = lqs.PossibleAnswers == null ? null : GetAnswerSet(lqs, false)
            }
        });
        return Ok(questions);
    }

    private List<ChallengeQuestionAnswerDTO>? GetAnswerSet(LearnQuestion lqs, bool firstSet)
    {
        switch (lqs.QuestionType)
        {
            case QuestionType.Distribute:
                string answerSet = lqs.RightAnswer.Split("|||")[firstSet ? 0 : 1];
                var answersDistribute = answerSet.Split("||").Select(ans => new ChallengeQuestionAnswerDTO
                {
                    AnswerId = new Guid(ans.Split("|")[0]),
                    Answer = ans.Split("|")[1]
                })
                .ToList();
                return answersDistribute;
            case QuestionType.MultipleChoice:
                if (firstSet)
                {
                    var answerMultipleChoice = lqs.RightAnswer.Split("||").Select(ans => new ChallengeQuestionAnswerDTO
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