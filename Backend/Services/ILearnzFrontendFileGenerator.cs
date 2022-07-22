namespace Learnz.Services;
public interface ILearnzFrontendFileGenerator
{
    FileFrontendDTO AnonymousFrontendFile(LearnzFileAnonymous file);
    FileFrontendDTO FrontendFile(LearnzFile file);
    FileFrontendHistorizedDTO FrontendFileHistorized(LearnzFile file, Guid guid);
    FileFrontendDTO FrontendFileFromVersion(LearnzFileVersion file);
    string PathToImage(string path);
}