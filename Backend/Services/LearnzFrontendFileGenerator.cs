namespace Learnz.Services;
public class LearnzFrontendFileGenerator : ILearnzFrontendFileGenerator
{
    public FileFrontendDTO FrontendFile(LearnzFile file)
    {
        return new FileFrontendDTO
        {
            Path = file.ActualVersionPath,
            ExternalFilename = file.ActualVersionFileNameExternal,
            ByteString = PathToImage(file.ActualVersionPath)
        };
    }

    public FileFrontendDTO FrontendFileFromVersion(LearnzFileVersion file)
    {
        return new FileFrontendDTO
        {
            Path = file.Path,
            ExternalFilename = file.FileNameExternal,
            ByteString = PathToImage(file.Path)
        };
    }

    public FileFrontendDTO AnonymousFrontendFile(LearnzFileAnonymous file)
    {
        return new FileFrontendDTO
        {
            Path = file.Path,
            ExternalFilename = file.FileNameExternal,
            ByteString = PathToImage(file.Path)
        };
    }

    public string PathToImage(string path)
    {
        if (File.Exists(path))
        {
            return "data:image/png;base64," + Convert.ToBase64String(File.ReadAllBytes(path));
        }
        return "";
    }
}
