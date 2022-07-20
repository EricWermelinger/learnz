namespace Learnz.Services
{
    public interface ILearnzFrontendFileGenerator
    {
        FileFrontendDTO AnonymousFrontendFile(LearnzFileAnonymous file);
        FileFrontendDTO FrontendFile(LearnzFile file);
        FileFrontendDTO FrontendFileFromVersion(LearnzFileVersion file);
        string PathToImage(string path);
    }
}