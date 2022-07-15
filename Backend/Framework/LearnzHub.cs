using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace Learnz.Framework;

[Authorize]
public class LearnzHub : Hub
{
    private readonly DataContext _dataContext;
    private readonly IUserService _userService;
    LearnzHub(DataContext dataContext, IUserService userService)
    {
        _dataContext = dataContext;
        _userService = userService;
    }

    private async Task Send(string endpoint, object data, string connectionId)
    {
        await Clients.Client(connectionId).SendAsync(endpoint, data);
    }

    public async Task SendToUser(string endpoint, object data, User user)
    {
        var connectionIds = await _userService.GetConnectionsOfUser(user);
        foreach (var connectionId in connectionIds)
        {
            await Send(endpoint, data, connectionId);
        }
    }

    public override async Task OnConnectedAsync()
    {
        var userId = _userService.GetUserGuid();
        await _dataContext.WebSocketConnections.AddAsync(new WebSocketConnection
        {
            UserId = userId,
            ConnectionId = Context.ConnectionId,
            Created = DateTime.UtcNow
        });
        await _dataContext.SaveChangesAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        var connections = _dataContext.WebSocketConnections.Where(cnc => cnc.ConnectionId == Context.ConnectionId).ToListAsync();
        if (connections != null)
        {
            _dataContext.WebSocketConnections.RemoveRange((IEnumerable<WebSocketConnection>)connections);
            await _dataContext.SaveChangesAsync();
        }
    }
}
