using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Learnz.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class GroupLeave : Controller
{
    private readonly DataContext _dataContext;
    private readonly IUserService _userService;
    public GroupLeave(DataContext dataContext, IUserService userService)
    {
        _dataContext = dataContext;
        _userService = userService;
    }

    [HttpPost]
    public async Task<ActionResult> LeaveGroup(GroupLeaveDTO request)
    {
        var guid = _userService.GetUserGuid();
        var groupMember = await _dataContext.GroupMembers.FirstOrDefaultAsync(gm => gm.UserId == guid && gm.GroupId == request.GroupId);
        if (groupMember != null)
        {
            _dataContext.GroupMembers.Remove(groupMember);
            await _dataContext.SaveChangesAsync();
        }
        return Ok();
    }
}
