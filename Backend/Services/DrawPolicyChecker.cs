namespace Learnz.Services;
public class DrawPolicyChecker : IDrawPolicyChecker
{
    public bool GroupPageEditable(DrawGroupCollection collection, Guid userId)
    {
        switch (collection.DrawGroupPolicy)
        {
            case DrawGroupPolicy.OnlySelfEditable:
                return collection.DrawCollection.OwnerId == userId;
            case DrawGroupPolicy.Public:
                return true;
            default:
                return false;
        }
    }

    public bool GroupPageDeletable(DrawGroupCollection collection, Guid userId)
    {
        switch (collection.DrawGroupPolicy)
        {
            case DrawGroupPolicy.OnlySelfEditable:
            case DrawGroupPolicy.Public:
                return collection.DrawCollection.OwnerId == userId;
            default:
                return false;
        }
    }
}