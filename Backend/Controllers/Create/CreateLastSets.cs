using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Learnz.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class CreateLastSets : Controller
{
    private readonly DataContext _dataContext;
    private readonly IUserService _userService;
    private readonly ISetPolicyChecker _setPolicyChecker;
    private readonly ICreateQueryService _createQueryService;
    public CreateLastSets(DataContext dataContext, IUserService userService, ISetPolicyChecker setPolicyChecker, ICreateQueryService createQueryService)
    {
        _dataContext = dataContext;
        _userService = userService;
        _setPolicyChecker = setPolicyChecker;
        _createQueryService = createQueryService;
    }

    [HttpGet]
    public async Task<ActionResult<List<CreateSetOverviewDTO>>> GetSets()
    {
        var guid = _userService.GetUserGuid();
        var setsTest = await _dataContext.Tests.Where(tst => tst.OwnerId == guid || tst.TestOfUsers.Any(tou => tou.UserId == guid))
                                               .Select(tst => new KeyValuePair<Guid, DateTime>(tst.SetId, tst.TestOfUsers.Any(tou => tou.UserId == guid) ? tst.TestOfUsers.First(tou => tou.UserId == guid).Ended ?? tst.Created : tst.Created))
                                               .ToListAsync();
        var setsChallenge = await _dataContext.Challenges.Where(chl => chl.ChallengeUsers.Any(chu => chu.UserId == guid) || chl.OwnerId == guid)
                                                         .Select(chl => new KeyValuePair<Guid, DateTime>(chl.CreateSetId, chl.ChallengeQuestionsPosed.Max(chl => chl.Created)))
                                                         .ToListAsync();
        var setsLearn = await _dataContext.LearnSessions.Where(lss => lss.UserId == guid)
                                                        .Select(lss => new KeyValuePair<Guid, DateTime>(lss.SetId, lss.Ended ?? lss.Created))
                                                        .ToListAsync();
        var setsAll = setsTest.Concat(setsChallenge)
                           .Concat(setsLearn)
                           .GroupBy(set => set.Key)
                           .Select(set => new KeyValuePair<Guid, DateTime>(set.Key, set.Max(s => s.Value)))
                           .ToList();

        var sets = await _dataContext.CreateSets.Where(crs => setsAll.Select(s => s.Key).Contains(crs.Id))
                                                .Include(crs => crs.CreatedBy)
                                                .ToListAsync();
        var setsWithPolicy = sets.Select(crs => new CreateSetOverviewDTO
        {
            SetId = crs.Id,
            Description = crs.Description,
            Name = crs.Name,
            Owner = crs.CreatedBy.Username,
            SubjectMain = crs.SubjectMain,
            SubjectSecond = crs.SubjectSecond,
            NumberOfQuestions = _createQueryService.NumberOfWords(crs),
            Usable = _setPolicyChecker.SetUsable(crs, guid),
            Editable = _setPolicyChecker.SetEditable(crs, guid),
            PolicyEditable = _setPolicyChecker.SetPolicyEditable(crs, guid)
        })
        .Where(set => set.Usable)
        .OrderByDescending(set => setsAll.First(s => s.Key == set.SetId).Value)
        .Take(20)
        .ToList();

        return Ok(setsWithPolicy);
    }
}
