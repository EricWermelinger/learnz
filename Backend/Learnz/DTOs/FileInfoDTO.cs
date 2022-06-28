namespace Learnz.DTOs;
public class FileInfoDTO
{
    public string FileNameExternal { get; set; }
    public string FilePath { get; set; }
    public DateTime Created { get; set; }
    public string CreatedUsername { get; set; }
    public DateTime Modified { get; set; }
    public string ModifiedUsername { get; set; }
    public bool FileFromMe { get; set; }
    public bool FileEditable { get; set; }
    public bool FileDeletable { get; set; }
}
