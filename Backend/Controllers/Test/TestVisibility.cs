using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Learnz.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class TestVisibility : Controller
{
    private readonly DataContext _dataContext;
    private readonly IUserService _userService;
    public TestVisibility(DataContext dataContext, IUserService userService)
    {
        _dataContext = dataContext;
        _userService = userService;
    }

    [HttpPost]
    public async Task<ActionResult> SetVisibility(TestVisibilityDTO request)
    {
        var guid = _userService.GetUserGuid();
        var test = await _dataContext.Tests.FirstOrDefaultAsync(tst => tst.Id == request.TestId && tst.OwnerId == guid);
        if (test == null)
        {
            return BadRequest(ErrorKeys.TestNotAccessible);
        }
        test.Visible = request.Visible;
        await _dataContext.SaveChangesAsync();
        return Ok();
    }
}
