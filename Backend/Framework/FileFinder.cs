namespace Learnz.Framework;
public class FileFinder : IFileFinder
{
    public async Task<Guid?> GetFileId(DataContext dataContext, Guid userId, string path, IFilePolicyChecker policyChecker)
    {
        var file = await dataContext.Files.FirstOrDefaultAsync(f =>
            f.Path == path && policyChecker.FileDownloadable(f, userId));
        return file?.Id;
    }
}
