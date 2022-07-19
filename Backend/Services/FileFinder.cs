namespace Learnz.Services;
public class FileFinder : IFileFinder
{
    public async Task<Guid?> GetFileId(DataContext dataContext, Guid userId, string path, IFilePolicyChecker policyChecker)
    {
        var file = await dataContext.Files.FirstOrDefaultAsync(f => f.ActualVersionPath == path);
        if (file != null)
        {
            if (policyChecker.FileDownloadable(file, userId))
            {
                return file.Id;
            }
        }
        return null;
    }
}
