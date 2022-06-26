namespace Learnz.Framework;
public interface IFilePolicyChecker
{
    public bool FileEditable(LearnzFile file, Guid userId);
    public bool FileDeletable(LearnzFile file, Guid userId);
}
