using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Learnz.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class GroupMessages : Controller
{
    private readonly DataContext _dataContext;
    private readonly IUserService _userService;
    public GroupMessages(DataContext dataContext, IUserService userService)
    {
        _dataContext = dataContext;
        _userService = userService;
    }

    [HttpGet]
    public async Task<ActionResult<List<GroupMessageGetDTO>>> GetMessages(Guid groupId)
    {
        var guid = _userService.GetUserGuid();
        var messages = await _dataContext.GroupMessages.Where(msg => msg.GroupId == groupId)
            .OrderByDescending(msg => msg.Date)
            .Select(msg => new GroupMessageGetDTO
            {
                Message = msg.Message,
                UserName = msg.Sender.Firstname + " " + msg.Sender.Lastname,
                Date = msg.Date,
                SentByMe = msg.SenderId == guid,
                IsInfoMessage = msg.IsInfoMessage
            })
            .ToListAsync();
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
        return Ok();
    }
}
