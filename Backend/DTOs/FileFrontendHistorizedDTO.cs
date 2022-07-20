namespace Learnz.DTOs;
public class FileFrontendHistorizedDTO
{
    public string Path { get; set; }
    public string ExternalFilename { get; set; }
    public string ByteString { get; set; }
    public bool Editable { get; set; }
    public bool PolicyChangeable { get; set; }
    public bool Deletable { get; set; }
}