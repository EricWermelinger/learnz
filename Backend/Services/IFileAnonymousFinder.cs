namespace Learnz.Services;
public interface IFileAnonymousFinder
{
    public Task<Guid?> GetFileId(DataContext dataContext, string path);
}
