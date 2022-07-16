using Microsoft.AspNetCore.SignalR;

namespace Learnz.Framework;

public class LearnzHub : Hub
{
    public string GetConnectionId()
    {
        return Context.ConnectionId;
    }

    public override Task OnConnectedAsync()
    {
        HubConnections.AddConnection(Context.ConnectionId);
        return base.OnConnectedAsync();
    }

    public override Task OnDisconnectedAsync(Exception? exception)
    {
        HubConnections.DeleteConnection(Context.ConnectionId);
        return base.OnDisconnectedAsync(exception);
    }
}
