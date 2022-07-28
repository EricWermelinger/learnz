using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Learnz.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class UserLanguage : Controller
{
    private readonly DataContext _dataContext;
    private readonly IUserService _userService;
    public UserLanguage(DataContext dataContext, IUserService userService)
    {
        _dataContext = dataContext;
        _userService = userService;
    }

    [HttpPost]
    public async Task<ActionResult> SetLanguage(UserLanguageDTO request)
    {
        var guid = _userService.GetUserGuid();
        var user = await _dataContext.Users.Where(usr => usr.Id == guid).FirstAsync();
        user.Language = request.Language;
        await _dataContext.SaveChangesAsync();
        return Ok();
    }
}
