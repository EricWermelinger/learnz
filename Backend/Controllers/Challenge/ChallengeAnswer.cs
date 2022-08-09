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
        bool isRight = false;
        // todo continue here
    }
}
