namespace Learnz.Services;
public interface IDrawQueryService
{
    Task<List<DrawCollectionGetDTO>> GetCollections(Guid userId);
    Task<List<DrawPageGetDTO>?> GetPages(Guid userId, Guid collectionId);
    Task TriggerWebsocketCollections(Guid collectionChangedId, Guid? groupId, Guid? userId);
    Task TriggerWebsocketPages(Guid collectionChangedId);
}