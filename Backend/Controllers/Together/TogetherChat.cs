using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace Learnz.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class TogetherChat : Controller
{
    private readonly DataContext _dataContext;
    private readonly IUserService _userService;
    private readonly ITogetherQueryService _togetherQueryService;
    private readonly HubService _hubService;
    public TogetherChat(DataContext dataContext, IUserService userService, ITogetherQueryService togetherQueryService, IHubContext<LearnzHub> learnzHub)
    {
        _dataContext = dataContext;
        _userService = userService;
        _togetherQueryService = togetherQueryService;
        _hubService = new HubService(learnzHub);
    }

    [HttpGet]
    public async Task<ActionResult<TogetherChatDTO>> GetMessages(Guid userId)
    {
        var guid = _userService.GetUserGuid();
        var messages = await _togetherQueryService.GetMessages(guid, userId);
        return Ok(messages);
    }

    [HttpPost]
    public async Task<ActionResult> SendMessage(TogetherChatSendMessageDTO request)
    {
        var guid = _userService.GetUserGuid();
        var message = new TogetherMessage
        {
            Message = request.Message,
            Date = DateTime.UtcNow,
            SenderId = guid,
            ReceiverId = request.UserId
        };
        await _dataContext.TogetherMessages.AddAsync(message);
        await _dataContext.SaveChangesAsync();

        var messageUser1 = await _togetherQueryService.GetMessages(guid, request.UserId);
        var messageUser2 = await _togetherQueryService.GetMessages(request.UserId, guid);
        await _hubService.SendMessageToUser(nameof(TogetherChat), messageUser1, guid, request.UserId);
        await _hubService.SendMessageToUser(nameof(TogetherChat), messageUser2, request.UserId, guid);
        var connectionsUser1 = await _togetherQueryService.GetConnectionOverview(guid);
        var connectionsUser2 = await _togetherQueryService.GetConnectionOverview(request.UserId);
        await _hubService.SendMessageToUser(nameof(TogetherConnectUser), connectionsUser1, guid);
        await _hubService.SendMessageToUser(nameof(TogetherConnectUser), connectionsUser2, request.UserId);

        return Ok();
    }
}
