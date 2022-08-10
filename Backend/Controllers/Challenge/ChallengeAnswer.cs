using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Learnz.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class ChallengeAnswer : Controller
{
    private readonly DataContext _dataContext;
    private readonly IUserService _userService;
    public ChallengeAnswer(DataContext dataContext, IUserService userService)
    {
        _dataContext = dataContext;
        _userService = userService;
    }

    [HttpPost]
    public async Task<ActionResult> Answer(ChallengeAnswerDTO request)
    {
        var guid = _userService.GetUserGuid();
        var challenge = await _dataContext.Challenges.Include(chl => chl.ChallengeQuestionAnswers).Include(chl => chl.ChallengeQuestionsPosed).FirstOrDefaultAsync(chl => chl.ChallengeUsers.Select(chu => chu.UserId).Contains(guid));
        if (challenge == null)
        {
            return BadRequest(ErrorKeys.ChallengeNotAccessible);
        }
        if (challenge.ChallengeQuestionAnswers.Any(ans => ans.UserId == guid && ans.ChallengeQuestionPosedId == request.QuestionId))
        {
            return BadRequest(ErrorKeys.ChallengeAlreadyAnswered);
        }
        var question = challenge.ChallengeQuestionsPosed.FirstOrDefault(qst => qst.Id == request.QuestionId && qst.IsActive == true);
        if (question == null)
        {
            return BadRequest(ErrorKeys.ChallengeAnswerNotPossible);
        }
        
        bool isRight = question.Answer.ToLower().Trim() == request.Answer.ToLower().Trim();
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

        // todo trigger websocket answered

        int peopleAnswered = await _dataContext.ChallengeQuestionAnswers.Where(cqa => cqa.ChallengeQuestionPosedId == question.Id).DistinctBy(cqa => cqa.UserId).CountAsync();
        int playerInGame = await _dataContext.ChallengeUsers.Where(chu => chu.ChallengeId == challenge.Id).CountAsync();
        if (peopleAnswered == playerInGame)
        {
            // todo change to result, set question inactive
            // todo trigger websocket result
        }

        return Ok();
    }
}
