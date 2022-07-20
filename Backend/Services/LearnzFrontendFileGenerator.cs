namespace Learnz.Services;
public class LearnzFrontendFileGenerator : ILearnzFrontendFileGenerator
{
    public LearnzFileFrontendDTO FrontendFile(LearnzFile file)
    {
        return new LearnzFileFrontendDTO
        {
            Path = file.ActualVersionPath,
            ExternalFilename = file.ActualVersionFileNameExternal,
            ByteString = PathToImage(file.ActualVersionPath)
        };
    }

    public LearnzFileFrontendDTO FrontendFileFromVersion(LearnzFileVersion file)
    {
        return new LearnzFileFrontendDTO
        {
            Path = file.Path,
            ExternalFilename = file.FileNameExternal,
            ByteString = PathToImage(file.Path)
        };
    }

    public LearnzFileFrontendDTO AnonymousFrontendFile(LearnzFileAnonymous file)
    {
        return new LearnzFileFrontendDTO
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
