namespace Learnz.Entities;
public class LearnzFileVersion
{
    public Guid Id { get; set; }
    public Guid FileId { get; set; }
    public LearnzFile File { get; set; }
    public string FileNameInternal { get; set; }
    public string FileNameExternal { get; set; }
    public string Path { get; set; }
    public DateTime Created { get; set; }
    public Guid CreatedById { get; set; }
    public User CreatedBy { get; set; }
}