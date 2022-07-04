using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Learnz.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class UserDarkTheme : Controller
{
    private readonly DataContext _dataContext;
    private readonly IUserService _userService;
    public UserDarkTheme(DataContext dataContext, IUserService userService)
    {
        _dataContext = dataContext;
        _userService = userService;
    }

    [HttpPost]
    public async Task<ActionResult> SetDarkTheme(UserDarkThemeDTO request)
    {
        var guid = _userService.GetUserGuid();
        var user = await _dataContext.Users.Where(usr => usr.Id == guid).FirstAsync();
        user.DarkTheme = request.DarkTheme;
        await _dataContext.SaveChangesAsync();
        return Ok();
    }

}
