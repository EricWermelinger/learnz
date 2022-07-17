using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace Learnz.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class GroupLeave : Controller
{
    private readonly DataContext _dataContext;
    private readonly IUserService _userService;
    private readonly IGroupQueryService _groupQueryService;
    private readonly HubService _hubService;
    public GroupLeave(DataContext dataContext, IUserService userService, IGroupQueryService groupQueryService, IHubContext<LearnzHub> learnzHub)
    {
        _dataContext = dataContext;
        _userService = userService;
        _groupQueryService = groupQueryService;
        _hubService = new HubService(learnzHub);
    }

    [HttpPost]
    public async Task<ActionResult> LeaveGroup(GroupLeaveDTO request)
    {
        var user = await _userService.GetUser();
        var guid = user.Id;
        var groupMember = await _dataContext.GroupMembers.FirstOrDefaultAsync(gm => gm.UserId == guid && gm.GroupId == request.GroupId);
        if (groupMember != null)
        {
            _dataContext.GroupMembers.Remove(groupMember);
            var leftMessage = new GroupMessage
            {
                GroupId = request.GroupId,
                IsInfoMessage = true,
                Date = DateTime.UtcNow,
                Message = "userLeft|" + user.Username,
                SenderId = guid
            };
            _dataContext.GroupMessages.Add(leftMessage);
            await _dataContext.SaveChangesAsync();
        }

        var groupMembersIds = await _dataContext.GroupMembers.Where(gm => gm.GroupId == request.GroupId)
                                                             .Select(gm => gm.UserId)
                                                             .ToListAsync();
        foreach (var memberId in groupMembersIds)
        {
            var groupOverview = await _groupQueryService.GetGroupOverview(memberId);
            var chatMessages = await _groupQueryService.GetMessages(memberId, request.GroupId);
            await _hubService.SendMessageToUser(nameof(GroupOverview), groupOverview, memberId);
            await _hubService.SendMessageToUser(nameof(GroupOverview), chatMessages, memberId, request.GroupId);
        }

        return Ok();
    }
}
