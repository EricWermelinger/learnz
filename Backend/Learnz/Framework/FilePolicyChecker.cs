namespace Learnz.Framework;
public class FilePolicyChecker : IFilePolicyChecker
{
    public bool FileEditable(LearnzFile file, Guid userId)
    {
        switch (file.FilePolicy)
        {
            case FilePolicy.Everyone:
                return true;
            case FilePolicy.OnlySelf:
                return file.CreatedById == userId;
            default:
                return false;
        }
    }

    public bool FileDeletable(LearnzFile file, Guid userId)
    {
        return file.CreatedById == userId;
    }

    public bool FileDownloadable(LearnzFile file, Guid userId)
    {
        return FileEditable(file, userId);
    }

    public bool FilePolicyChangeable(LearnzFile file, Guid userId)
    {
        return FileDeletable(file, userId);
    }
}
