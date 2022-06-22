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
        var openAsks = await _dataContext.TogetherAsk.Where(ask => ask.Answer == null && ask.UserId2 == guid)
                                                     .Select(ask => ask.UserId1)
                                                     .ToListAsync();
        var users = await _dataContext.Users.Where(usr => openAsks.Contains(usr.Id))
            .Select(usr => new TogetherUserProfileDTO
            {
                UserId = usr.Id,
                Username = usr.Username,
                Grade = usr.Grade,
                ProfileImage = usr.ProfileImage,
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
            UserId1 = guid ?? Guid.NewGuid(),
            UserId2 = request.UserId,
            Answer = null
        };

        var exists = await _dataContext.TogetherAsk.AnyAsync(ask => ask.UserId1 == newAsk.UserId1 && ask.UserId2 == newAsk.UserId2);
        if (exists)
        {
            return BadRequest("alreadyAsked");
        }

        await _dataContext.TogetherAsk.AddAsync(newAsk);
        await _dataContext.SaveChangesAsync();

        return Ok();
    }

    [HttpPut]
    public async Task<ActionResult> AnswerAsk(TogetherAskAnswerDTO request)
    {
        var guid = _userService.GetUserGuid();
        var ask = await _dataContext.TogetherAsk.FirstAsync(ask => ask.UserId1 == request.UserId && ask.UserId2 == guid);
        ask.Answer = request.Answer;
        await _dataContext.SaveChangesAsync();
        if (request.Answer)
        {
            await _dataContext.TogetherConnections.AddAsync(new TogetherConnection
            {
                Id = Guid.NewGuid(),
                UserId1 = request.UserId,
                UserId2 = guid ?? Guid.NewGuid()
            });
            await _dataContext.SaveChangesAsync();
        }
        return Ok();
    }
}
