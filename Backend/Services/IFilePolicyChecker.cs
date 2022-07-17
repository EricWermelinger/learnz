namespace Learnz.Services;
public interface IFilePolicyChecker
{
    public bool FileEditable(LearnzFile file, Guid userId);
    public bool FileDeletable(LearnzFile file, Guid userId);
    public bool FileDownloadable(LearnzFile file, Guid userId);
    public bool FilePolicyChangeable(LearnzFile file, Guid userId);
}
