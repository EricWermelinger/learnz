namespace Learnz.Services;
public class FilePolicyChecker : IFilePolicyChecker
{
    public bool FileEditable(LearnzFile file, Guid userId)
    {
        switch (file.FilePolicy)
        {
            case FilePolicy.Everyone:
                return true;
            case FilePolicy.Private:
            case FilePolicy.OnlySelfEditable:
                return file.OwnerId == userId;
            default:
                return false;
        }
    }

    public bool FileDeletable(LearnzFile file, Guid userId)
    {
        switch (file.FilePolicy)
        {
            case FilePolicy.Everyone:
            case FilePolicy.OnlySelfEditable:
            case FilePolicy.Private:
                return file.OwnerId == userId;
            default:
                return false;
        }
    }

    public bool FileDownloadable(LearnzFile file, Guid userId)
    {
        switch (file.FilePolicy)
        {
            case FilePolicy.Everyone:
            case FilePolicy.OnlySelfEditable:
                return true;
            case FilePolicy.Private:
                return file.OwnerId == userId;
            default:
                return false;
        }
    }

    public bool FilePolicyChangeable(LearnzFile file, Guid userId)
    {
        switch (file.FilePolicy)
        {
            case FilePolicy.Everyone:
            case FilePolicy.OnlySelfEditable:
                return true;
            case FilePolicy.Private:
                return file.OwnerId == userId;
            default:
                return false;
        }
    }
}
