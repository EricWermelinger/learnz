namespace Learnz.Framework;

public class PathToImageConverter : IPathToImageConverter
{
    public string PathToImage(string path)
    {
        if (File.Exists(path))
        {
            return "data:image/png;base64," + Convert.ToBase64String(File.ReadAllBytes(path));
        }
        return "";
    }
}
