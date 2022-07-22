namespace Learnz.Services;
public class SetPolicyChecker : ISetPolicyChecker
{
    public bool SetDeletable(CreateSet set, Guid userId)
    {
        switch (set.SetPolicy)
        {
            case SetPolicy.Private:
            case SetPolicy.OnlySelfEditable:
            case SetPolicy.Everyone:
                return set.CreatedById == userId;
            default:
                return false;
        }
    }

    public bool SetEditable(CreateSet set, Guid userId)
    {
        switch (set.SetPolicy)
        {
            case SetPolicy.Private:
            case SetPolicy.OnlySelfEditable:
                return set.CreatedById == userId;
            case SetPolicy.Everyone:
                return true;
            default:
                return false;
        }
    }

    public bool SetUsable(CreateSet set, Guid userId)
    {
        switch (set.SetPolicy)
        {
            case SetPolicy.Private:
                return set.CreatedById == userId;
            case SetPolicy.OnlySelfEditable:
            case SetPolicy.Everyone:
                return true;
            default:
                return false;               
        }
    }
}
