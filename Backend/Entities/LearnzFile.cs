namespace Learnz.Entities;
public class LearnzFile
{
    public Guid Id { get; set; }
    public Guid ActualVersionId { get; set; }
    public string ActualVersionFileNameExternal { get; set; }
    public string ActualVersionPath { get; set; }
    public Guid OwnerId { get; set; }
    public User Owner { get; set; }
    public FilePolicy FilePolicy { get; set; }

    public ICollection<Group> GroupImageFiles { get; set; }
    public ICollection<GroupFile> GroupFiles { get; set; }
    public ICollection<LearnzFileVersion> LearnzFileVersions { get; set; }
}
