using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Learnz.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class TogetherAskUser : Controller
{
    private readonly DataContext _dataContext;
    private readonly IUserService _userService;
    public TogetherAskUser(DataContext dataContext, IUserService userService)
    {
        _dataContext = dataContext;
        _userService = userService;
    }

    [HttpGet]
    public async Task<ActionResult<List<TogetherUserProfileDTO>>> GetOpenAsks()
    {
        var guid = _userService.GetUserGuid();
        var users = await _dataContext.TogetherAsks.Where(ask => ask.Answer == null && ask.AskedUserId == guid)
                                                     .Select(ask => ask.InterestedUser)
                                                     .Select(usr => new TogetherUserProfileDTO
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
        return Ok(users);
    }

    [HttpPost]
    public async Task<ActionResult> AskUser(TogetherAskUserDTO request)
    {
        var guid = _userService.GetUserGuid();
        var newAsk = new TogetherAsk
        {
            Id = Guid.NewGuid(),
            InterestedUserId = guid,
            AskedUserId = request.UserId,
            Answer = null
        };

        var exists = await _dataContext.TogetherAsks.AnyAsync(ask => ask.InterestedUserId == newAsk.InterestedUserId && ask.AskedUserId == newAsk.AskedUserId);
        if (!exists)
        {
            await _dataContext.TogetherAsks.AddAsync(newAsk);
        }

        await _dataContext.SaveChangesAsync();

        return Ok();
    }

    [HttpPut]
    public async Task<ActionResult> AnswerAsk(TogetherAskAnswerDTO request)
    {
        var guid = _userService.GetUserGuid();
        var ask = await _dataContext.TogetherAsks.FirstAsync(ask => ask.InterestedUserId == request.UserId && ask.AskedUserId == guid);
        ask.Answer = request.Answer;
        await _dataContext.SaveChangesAsync();
        if (request.Answer)
        {
            var exists = await _dataContext.TogetherConnections.AnyAsync(cnc =>
                (cnc.UserId1 == guid && cnc.UserId2 == request.UserId) ||
                (cnc.UserId1 == request.UserId && cnc.UserId2 == guid));

            if (!exists)
            {
                await _dataContext.TogetherConnections.AddAsync(new TogetherConnection
                {
                    Id = Guid.NewGuid(),
                    UserId1 = request.UserId,
                    UserId2 = guid
                });
                await _dataContext.SaveChangesAsync();
            }
        }
        return Ok();
    }
}
