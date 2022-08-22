using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Learnz.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class TestGroupTestVisibility : Controller
{
    private readonly DataContext _dataContext;
    private readonly IUserService _userService;
    public TestGroupTestVisibility(DataContext dataContext, IUserService userService)
    {
        _dataContext = dataContext;
        _userService = userService;
    }

    [HttpPost]
    public async Task<ActionResult> SetVisiblity(TestVisibilityDTO request)
    {
        var guid = _userService.GetUserGuid();
        var test = await _dataContext.Tests.FirstOrDefaultAsync(tst => tst.OwnerId == guid && tst.Id == request.TestId);
        if (test == null)
        {
            return BadRequest(ErrorKeys.TestNotAccessible);
        }
        test.Visible = request.Visible;
        await _dataContext.SaveChangesAsync();
        return Ok();
    }
}
