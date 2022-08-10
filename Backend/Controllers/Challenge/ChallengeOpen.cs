using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Learnz.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class ChallengeOpen : Controller
{
    private readonly DataContext _dataContext;
    private readonly IUserService _userService;
    private readonly ISetPolicyChecker _setPolicyChecker;
    private readonly IChallengeQueryService _challengeQueryService;
    public ChallengeOpen(DataContext dataContext, IUserService userService, ISetPolicyChecker setPolicyChecker, IChallengeQueryService challengeQueryService)
    {
        _dataContext = dataContext;
        _userService = userService;
        _setPolicyChecker = setPolicyChecker;
        _challengeQueryService = challengeQueryService;
    }

    [HttpGet]
    public async Task<ActionResult<List<ChallengeOpenDTO>>> OpenChallenges()
    {
        var challenges = await _dataContext.Challenges.Where(chl => chl.State == ChallengeState.BeforeGame)
                                                .Select(chl => new ChallengeOpenDTO
                                                {
                                                    ChallengeId = chl.Id,
                                                    Name = chl.Name,
                                                    CreateSetName = chl.CreateSet.Name,
                                                    SubjectMain = chl.CreateSet.SubjectMain,
                                                    SubjectSecond = chl.CreateSet.SubjectSecond,
                                                    NumberOfPlayers = chl.ChallengeUsers.Count
                                                })
                                                .ToListAsync();
        return Ok(challenges);
    }

    [HttpPost]
    public async Task<ActionResult> CreateChallenge(ChallengeCreateDTO request)
    {
        var guid = _userService.GetUserGuid();
        var existing = await _dataContext.Challenges.FirstOrDefaultAsync(chl => chl.Id == request.ChallengeId);
        if (existing != null)
        {
            return BadRequest(ErrorKeys.ChallengeNotAccessible);
        }
        var set = await _dataContext.CreateSets.FirstOrDefaultAsync(set => set.Id == request.CreateSetId);
        if (set == null || _setPolicyChecker.SetUsable(set, guid))
        {
            return BadRequest(ErrorKeys.SetNotAccessible);
        }
        var challenge = new Challenge
        {
            Id = request.ChallengeId,
            Name = request.Name,
            CreateSetId = request.CreateSetId,
            OwnerId = guid,
            State = ChallengeState.BeforeGame
        };
        _dataContext.Challenges.Add(challenge);
        await _dataContext.SaveChangesAsync();
        return Ok();
    }

    [HttpPut]
    public async Task<ActionResult> JoinChallenge(ChallengeIdDTO request)
    {
        var guid = _userService.GetUserGuid();
        var challenge = await _dataContext.Challenges.FirstOrDefaultAsync(chl => chl.State == ChallengeState.BeforeGame
                                                                            && !chl.ChallengeUsers.Select(chu => chu.UserId).Contains(guid)
                                                                            && chl.OwnerId != guid);
        if (challenge == null)
        {
            return BadRequest(ErrorKeys.ChallengeNotAccessible);
        }
        var challengeUser = new ChallengeUser
        {
            ChallengeId = request.ChallengeId,
            UserId = guid
        };
        _dataContext.ChallengeUsers.Add(challengeUser);
        await _dataContext.SaveChangesAsync();
        await _challengeQueryService.TriggerWebsocketAllUsers(challenge.Id);
        return Ok();
    }

    [HttpDelete]
    public async Task<ActionResult> DeleteChallenge(Guid challengeId)
    {
        var guid = _userService.GetUserGuid();
        var challenge = await _dataContext.Challenges.FirstOrDefaultAsync(chl => chl.OwnerId == guid && chl.Id == challengeId);
        if (challenge == null)
        {
            return BadRequest(ErrorKeys.ChallengeNotAccessible);
        }
        challenge.State = ChallengeState.Ended;
        await _dataContext.SaveChangesAsync();
        await _challengeQueryService.TriggerWebsocketAllUsers(challengeId);
        return Ok();
    }
}
