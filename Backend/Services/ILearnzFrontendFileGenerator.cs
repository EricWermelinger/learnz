namespace Learnz.Services
{
    public interface ILearnzFrontendFileGenerator
    {
        LearnzFileFrontendDTO AnonymousFrontendFile(LearnzFileAnonymous file);
        LearnzFileFrontendDTO FrontendFile(LearnzFile file);
        string PathToImage(string path);
    }
}