using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Learnz.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class GroupPossibleUsers : Controller
{
    private readonly DataContext _dataContext;
    private readonly IUserService _userService;
    public GroupPossibleUsers(DataContext dataContext, IUserService userService)
    {
        _dataContext = dataContext;
        _userService = userService;
    }

    [HttpGet]
    public async Task<ActionResult<List<GroupPossibleUserDTO>>> GetPossibleUsers()
    {
        var guid = _userService.GetUserGuid();
        var possibleUsers = await _dataContext.TogetherConnections.Where(cnc => cnc.UserId1 == guid || cnc.UserId2 == guid)
                                                                  .Select(cnc => cnc.UserId1 == guid ? cnc.User2 : cnc.User1)
                                                                  .Select(usr => new GroupPossibleUserDTO
                                                                  {
                                                                      UserId = usr.Id,
                                                                      Username = usr.Username,
                                                                  })
                                                                  .ToListAsync();
        return Ok(possibleUsers);
    }
}
