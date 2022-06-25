namespace Learnz.Entities;
public class LearnzFile
{
    public Guid Id { get; set; }
    public string FileNameInternal { get; set; }
    public string FileNameExternal { get; set; }
    public string Path { get; set; }
    public DateTime Created { get; set; }
    public Guid CreatedById { get; set; }
    public User CreatedBy { get; set; }
    public DateTime Modified { get; set; }
    public Guid ModifiedById { get; set; }
    public User ModifiedBy { get; set; }
    
    public ICollection<Group> GroupImageFiles { get; set; }
    public ICollection<GroupFile> GroupFiles { get; set; }
}
