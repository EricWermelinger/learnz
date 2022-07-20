namespace Learnz.DTOs;
public class FileVersionInfoDTO
{
    public FileFrontendDTO FrontendFile { get; set; }
    public DateTime Created { get; set; }
    public string CreatedBy { get; set; }
}