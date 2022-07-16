using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace Learnz.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class TogetherAskUser : Controller
{
    private readonly DataContext _dataContext;
    private readonly IUserService _userService;
    private readonly ITogetherQueryService _togetherQueryService;
    private readonly HubService _hubService;
    public TogetherAskUser(DataContext dataContext, IUserService userService, ITogetherQueryService togetherQueryService, IHubContext<LearnzHub> learnzHub)
    {
        _dataContext = dataContext;
        _userService = userService;
        _togetherQueryService = togetherQueryService;
        _hubService = new HubService(learnzHub);
    }

    [HttpGet]
    public async Task<ActionResult<List<TogetherUserProfileDTO>>> GetOpenAsks()
    {
        var guid = _userService.GetUserGuid();
        var users = await _togetherQueryService.GetOpenAsks(guid);
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

        await TriggerAskWebsocket(guid);
        await TriggerAskWebsocket(request.UserId);
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

            var connectionsUser1 = await _togetherQueryService.GetConnectionOverview(guid);
            var connectionsUser2 = await _togetherQueryService.GetConnectionOverview(request.UserId);
            await _hubService.SendMessageToUser(nameof(TogetherConnectUser), connectionsUser1, guid);
            await _hubService.SendMessageToUser(nameof(TogetherConnectUser), connectionsUser2, request.UserId);
        }
        await TriggerAskWebsocket(guid);
        await TriggerAskWebsocket(request.UserId);
        return Ok();
    }

    private async Task TriggerAskWebsocket(Guid userId)
    {
        var openAsks = _togetherQueryService.GetOpenAsks(userId);
        await _hubService.SendMessageToUser(nameof(TogetherAskUser), openAsks, userId);
    }
}
