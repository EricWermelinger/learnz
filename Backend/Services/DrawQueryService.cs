using Microsoft.AspNetCore.SignalR;

namespace Learnz.Services;
public class DrawQueryService : IDrawQueryService
{
    private readonly DataContext _dataContext;
    private readonly HubService _hubService;
    public DrawQueryService(DataContext dataContext, IHubContext<LearnzHub> learnzHub)
    {
        _dataContext = dataContext;
        _hubService = new HubService(learnzHub);
    }
    
    public async Task<List<DrawCollectionGetDTO>?> GetCollections(Guid userId)
    {
        throw new NotImplementedException();
    }

    public async Task<List<DrawPageGetDTO>?> GetPages(Guid userId, Guid collectionId)
    {
        throw new NotImplementedException();
    }

    public Task TriggerWebsocketCollections(Guid collectionChangedId, Guid? groupid)
    {
        throw new NotImplementedException();
    }

    public Task TriggerWebsocketPages(Guid collectionChangedId)
    {
        throw new NotImplementedException();
    }
}