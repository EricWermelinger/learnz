using Microsoft.AspNetCore.SignalR;

namespace Learnz.Framework;
public class HubService
{
    private readonly IHubContext<LearnzHub> _learnzHub;
    public HubService(IHubContext<LearnzHub> learnzHub)
    {
        _learnzHub = learnzHub;
    }

    public async Task SendMessageToUser(string endpoint, object data, Guid userId)
    {
        string endpointSanitized = endpoint[0].ToString().ToLower() + endpoint.Substring(1);
        foreach (var connection in HubConnections.ConnectionsOfUser(userId))
        {
            await _learnzHub.Clients.Client(connection).SendAsync(endpointSanitized, data);
        }
    }
}
