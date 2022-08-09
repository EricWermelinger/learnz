using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Learnz.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class CreateOwnSets : Controller
{
    private readonly DataContext _dataContext;
    private readonly IUserService _userService;
    private readonly ISetPolicyChecker _setPolicyChecker;
    private readonly ICreateQueryService _createQueryService;
    public CreateOwnSets(DataContext dataContext, IUserService userService, ISetPolicyChecker setPolicyChecker, ICreateQueryService createQueryService)
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
        var sets = await _dataContext.CreateSets.Where(crs => crs.CreatedById == guid)
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
        .Where(set => set.Usable);
        return Ok(setsWithPolicy);
    }
}