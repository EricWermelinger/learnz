namespace Learnz.Services;
public class FileAnonymousFinder : IFileAnonymousFinder
{
    public async Task<Guid?> GetFileId(DataContext dataContext, string path)
    {
        var file = await dataContext.FilesAnonymous.FirstOrDefaultAsync(f => f.Path == path);
        return file?.Id;
    }
}
