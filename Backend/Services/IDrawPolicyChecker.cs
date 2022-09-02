namespace Learnz.Services;
public interface IDrawPolicyChecker
{
    bool GroupPageDeletable(DrawGroupPolicy policy, Guid ownerId, Guid userId);
    bool GroupPageEditable(DrawGroupPolicy policy, Guid ownerId, Guid userId);
}