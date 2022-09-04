namespace Learnz.Services;
public interface IDrawQueryService
{
    Task<List<DrawCollectionGetDTO>> GetCollections(Guid userId);
    Task<DrawDrawingDTO?> GetPages(Guid userId, Guid collectionId);
    Task AdjustChangedOnCollection(Guid collectionId, Guid changedById);
    Task TriggerWebsocketCollections(Guid collectionChangedId, Guid? groupId, Guid? userId);
    Task TriggerWebsocketPages(Guid collectionChangedId);
    Task TriggerWebsocketNewUserEditing(Guid collectionChangedId, Guid previousUserId, Guid newUserId);
}