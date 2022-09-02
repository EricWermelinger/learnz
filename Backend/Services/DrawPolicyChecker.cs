namespace Learnz.Services;
public class DrawPolicyChecker : IDrawPolicyChecker
{
    public bool GroupPageDeletable(DrawGroupPolicy policy, Guid ownerId, Guid userId)
    {
        switch (policy)
        {
            case DrawGroupPolicy.OnlySelfEditable:
            case DrawGroupPolicy.Public:
                return ownerId == userId;
            default:
                return false;
        }
    }

    public bool GroupPageEditable(DrawGroupPolicy policy, Guid ownerId, Guid userId)
    {
        switch (policy)
        {
            case DrawGroupPolicy.OnlySelfEditable:
                return ownerId == userId;
            case DrawGroupPolicy.Public:
                return true;
            default:
                return false;
        }
    }
}