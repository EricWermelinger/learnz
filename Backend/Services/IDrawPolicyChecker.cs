namespace Learnz.Services;
public interface IDrawPolicyChecker
{
    bool GroupPageDeletable(DrawGroupCollection collection, Guid userId);
    bool GroupPageEditable(DrawGroupCollection collection, Guid userId);
}