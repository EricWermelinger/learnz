
namespace Learnz.Services
{
    public interface IFilePolicyChecker
    {
        bool FileDeletable(LearnzFile file, Guid userId);
        bool FileDownloadable(LearnzFile file, Guid userId);
        bool FileEditable(LearnzFile file, Guid userId);
        bool FilePolicyChangeable(LearnzFile file, Guid userId);
    }
}