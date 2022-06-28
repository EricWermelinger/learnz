namespace Learnz.Entities;
public class GroupFile
{
    public Guid Id { get; set; }
    public Guid GroupId { get; set; }
    public Group Group { get; set; }
    public Guid FileId { get; set; }
    public LearnzFile File { get; set; }
}
