namespace Learnz.Entities;
public class LearnzFile
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string MimeType { get; set; }
    public byte[] Data { get; set; }
    public DateTime Created { get; set; }
    public Guid CreatedById { get; set; }
    public User CreatedBy { get; set; }
    public DateTime Modified { get; set; }
    public Guid ModifiedById { get; set; }
    public User ModifiedBy { get; set; }
    
    public ICollection<Group> GroupImageFiles { get; set; }
    public ICollection<GroupFile> GroupFiles { get; set; }
}
