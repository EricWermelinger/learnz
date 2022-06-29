using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Learnz.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class TogetherConnectUser : Controller
{
    private readonly DataContext _dataContext;
    private readonly IUserService _userService;
    public TogetherConnectUser(DataContext dataContext, IUserService userService)
    {
        _dataContext = dataContext;
        _userService = userService;
    }

    [HttpGet]
    public async Task<ActionResult<List<TogetherUserProfileDTO>>> GetConnections()
    {
        var guid = _userService.GetUserGuid();
        var users = await _dataContext.TogetherConnections.Where(cnc => cnc.UserId1 == guid || cnc.UserId2 == guid)
                                                               .Select(cnc => cnc.UserId1 == guid ? cnc.User2 : cnc.User1)
                                                               .Select(usr => new TogetherOverviewUserProfileDTO
                                                               {
                                                                   UserId = usr.Id,
                                                                   Username = usr.Username,
                                                                   Grade = usr.Grade,
                                                                   ProfileImagePath = usr.ProfileImage.Path,
                                                                   Information = usr.Information,
                                                                   GoodSubject1 = usr.GoodSubject1,
                                                                   GoodSubject2 = usr.GoodSubject2,
                                                                   GoodSubject3 = usr.GoodSubject3,
                                                                   BadSubject1 = usr.BadSubject1,
                                                                   BadSubject2 = usr.BadSubject2,
                                                                   BadSubject3 = usr.BadSubject3
                                                               })
                                                               .ToListAsync();

        var messages = await _dataContext.TogetherMessages.Where(msg => msg.SenderId == guid || msg.ReceiverId == guid)
            .ToListAsync();

        foreach (var user in users)
        {
            var message = messages.Where(msg => msg.SenderId == user.UserId || msg.ReceiverId == user.UserId)
                .OrderByDescending(msg => msg.Date)
                .FirstOrDefault();
            if (message != null)
            {
                user.LastMessage = message.Message;
                user.LastMessageDateSent = message.Date;
                user.LastMessageSentByMe = message.SenderId == guid;
            }
        }

        return Ok(users.OrderBy(usr => usr.LastMessageDateSent).ToList());
    }
}
