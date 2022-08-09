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
        // todo implement this endpoint
        return Ok();
    }
}
