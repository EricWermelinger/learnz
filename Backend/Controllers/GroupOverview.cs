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
    public GroupOverview(DataContext dataContext, IUserService userService)
    {
        _dataContext = dataContext;
        _userService = userService;
    }

    [HttpGet]
    public async Task<ActionResult<List<GroupOverviewDTO>>> GetGroups()
    {
        var guid = _userService.GetUserGuid();
        var overview = await _dataContext.Groups.Where(grp => grp.GroupMembers.Select(gm => gm.Id).Contains(guid))
            .Select(grp => new GroupOverviewDTO
            {
                GroupId = grp.Id,
                GroupName = grp.Name,
                ProfileImagePath = grp.ProfileImage.Path,
                LastMessage = grp.GroupMessages.Any()
                    ? grp.GroupMessages.OrderByDescending(grp => grp.Date).First().Message
                    : null,
                LastMessageDateSent = grp.GroupMessages.Any()
                    ? grp.GroupMessages.OrderByDescending(grp => grp.Date).First().Date
                    : null,
                LastMessageSentByMe = grp.GroupMessages.Any()
                    ? grp.GroupMessages.OrderByDescending(grp => grp.Date).First().SenderId == guid
                    : null,
                LastMessageWasInfoMessage = grp.GroupMessages.Any()
                    ? grp.GroupMessages.OrderByDescending(grp => grp.Date).First().IsInfoMessage
                    : null,
                NumberOfFiles = grp.GroupFiles.Count()
            })
            .ToListAsync();
        return Ok(overview);
    }
}
