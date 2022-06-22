namespace Learnz.Entities;
public class GroupFile
{
    public Guid Id { get; set; }
    public Guid GroupId { get; set; }
    public Group Group { get; set; }
    public Guid LearnzFileId { get; set; }
    public LearnzFile LearnzFile { get; set; }
}
