using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Learnz.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class TogetherChat : Controller
{
    private readonly DataContext _dataContext;
    private readonly IUserService _userService;
    public TogetherChat(DataContext dataContext, IUserService userService)
    {
        _dataContext = dataContext;
        _userService = userService;
    }

    [HttpGet]
    public async Task<ActionResult<List<TogetherChatMessageDTO>>> GetMessages(TogetherGetChatDTO request)
    {
        var guid = _userService.GetUserGuid();
        var messages = await _dataContext.TogetherMessages.Where(msg => msg.Sender == guid || msg.Receiver == guid)
            .Select(msg => new TogetherChatMessageDTO
            {
                Message = msg.Message,
                DateSent = msg.Date,
                SentByMe = msg.Sender == guid
            }).ToListAsync();
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
            Sender = guid ?? Guid.NewGuid(),
            Receiver = request.UserId
        };
        await _dataContext.TogetherMessages.AddAsync(message);
        await _dataContext.SaveChangesAsync();
        return Ok();
    }
}
