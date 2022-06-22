namespace Learnz.Entities;
public class Group
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public Guid LearnzFileId { get; set; }
    public LearnzFile LearnzFile { get; set; }
    public Guid UserId { get; set; }
    public User User { get; set; }
}
