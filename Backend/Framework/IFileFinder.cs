namespace Learnz.Framework;
public interface IFileFinder
{
    public Task<Guid?> GetFileId(DataContext dataContext, Guid userId, string path, IFilePolicyChecker policyChecker);
}
