namespace Learnz.Services;
public interface ISetPolicyChecker
{
    public bool SetEditable(CreateSet set, Guid userId);
    public bool SetUsable(CreateSet set, Guid userId);
    public bool SetDeletable(CreateSet set, Guid userId);
    public bool SetPolicyEditable(CreateSet set, Guid userId);
}
