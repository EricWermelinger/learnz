using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Learnz.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class GroupOverview : Controller
{
    private readonly DataContext _dataContext;
    private readonly IUserService _userService;
    private readonly ILearnzFrontendFileGenerator _learnzFrontendFileGenerator;
    private readonly IGroupQueryService _groupQueryService;
    public GroupOverview(DataContext dataContext, IUserService userService, ILearnzFrontendFileGenerator learnzFrontendFileGenerator, IGroupQueryService groupQueryService)
    {
        _dataContext = dataContext;
        _userService = userService;
        _learnzFrontendFileGenerator = learnzFrontendFileGenerator;
        _groupQueryService = groupQueryService;
    }

    [HttpGet]
    public async Task<ActionResult<List<GroupOverviewDTO>>> GetGroups()
    {
        var guid = _userService.GetUserGuid();
        var overview = await _groupQueryService.GetGroupOverview(guid);
        return Ok(overview);
    }
}
