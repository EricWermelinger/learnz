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
    public ChallengeFlow(DataContext dataContext, IUserService userService)
    {
        _dataContext = dataContext;
        _userService = userService;
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
                                    .Include(crs => crs.QuestionTextFields)
                                    .Include(crs => crs.QuestionTrueFalses)
                                    .Include(crs => crs.QuestionWords)
                                    .FirstOrDefaultAsync();
                                    
        switch (challenge.State)
        {
            case ChallengeState.BeforeGame:
                // todo pose question
                // todo trigger websocket
                break;
            case ChallengeState.Question:
                // todo make question inactive
                // todo trigger websocket active for result
                break;
            case ChallengeState.Result:
                // todo pose question
                // todo trigger websocket
                break;
            //case ChallengeState.
        }
        return Ok();
    }
}
