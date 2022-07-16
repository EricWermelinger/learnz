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
    public async Task<ActionResult<List<TogetherChatMessageDTO>>> GetMessages(Guid userId)
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

        var wsMessages = await _togetherQueryService.GetMessages(guid, request.UserId);
        var wsMessagesInverted = wsMessages.Select(msg => new TogetherChatMessageDTO
                                                        {
                                                            Message = msg.Message,
                                                            DateSent = msg.DateSent,
                                                            SentByMe = !msg.SentByMe
                                                        });
        await _hubService.SendMessageToUser(nameof(TogetherChat), wsMessages, guid);
        await _hubService.SendMessageToUser(nameof(TogetherChat), wsMessagesInverted, request.UserId);
        var connectionsUser1 = await _togetherQueryService.GetConnectionOverview(guid);
        var connectionsUser2 = await _togetherQueryService.GetConnectionOverview(request.UserId);
        await _hubService.SendMessageToUser(nameof(TogetherConnectUser), connectionsUser1, guid);
        await _hubService.SendMessageToUser(nameof(TogetherConnectUser), connectionsUser2, request.UserId);

        return Ok();
    }
}
