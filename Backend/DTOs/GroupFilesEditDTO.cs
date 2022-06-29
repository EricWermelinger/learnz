namespace Learnz.DTOs;
public class GroupFilesEditDTO
{
    public Guid GroupId { get; set; }
    public List<Guid> FilesAdded { get; set; }
    public List<Guid> FilesDeleted { get; set; }
}
