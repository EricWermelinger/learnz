using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace Learnz.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class GroupMessages : Controller
{
    private readonly DataContext _dataContext;
    private readonly IUserService _userService;
    private readonly IGroupQueryService _groupQueryService;
    private readonly HubService _hubService;
    public GroupMessages(DataContext dataContext, IUserService userService, IGroupQueryService groupQueryService, IHubContext<LearnzHub> learnzHub)
    {
        _dataContext = dataContext;
        _userService = userService;
        _groupQueryService = groupQueryService;
        _hubService = new HubService(learnzHub);
    }

    [HttpGet]
    public async Task<ActionResult<List<GroupMessageGetDTO>>> GetMessages(Guid groupId)
    {
        var guid = _userService.GetUserGuid();
        var messages = await _groupQueryService.GetMessages(guid, groupId);
        return Ok(messages);
    }

    [HttpPost]
    public async Task<ActionResult> SendMessage(GroupMessageSendDTO request)
    {
        var guid = _userService.GetUserGuid();
        var message = new GroupMessage
        {
            SenderId = guid,
            Message = request.Message,
            Date = DateTime.UtcNow,
            GroupId = request.GroupId,
            IsInfoMessage = false
        };
        await _dataContext.GroupMessages.AddAsync(message);
        await _dataContext.SaveChangesAsync();

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
