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
    private readonly IChallengeQueryService _challengeQueryService;
    public ChallengeActive(DataContext dataContext, IUserService userService, IChallengeQueryService challengeQueryService)
    {
        _dataContext = dataContext;
        _userService = userService;
        _challengeQueryService = challengeQueryService;
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

        var data = await _challengeQueryService.GetActiveChallenge(challenge, guid);
        return Ok(data);
    }
}