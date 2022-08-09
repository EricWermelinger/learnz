using Microsoft.AspNetCore.SignalR;

namespace Learnz.Services;
public class ChallengeQueryService
{
    private readonly DataContext _dataContext;
    private readonly HubService _hubService;
    public ChallengeQueryService(DataContext dataContext, IHubContext<LearnzHub> learnzHub)
    {
        _dataContext = dataContext;
        _hubService = new HubService(learnzHub);
    }

    private void TriggerWebsocket()
    {

    }
}