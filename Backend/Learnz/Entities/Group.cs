namespace Learnz.Entities;
public class Group
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public Guid FileId { get; set; }
    public LearnzFile File { get; set; }
    public Guid UserId { get; set; }
    public User User { get; set; }
   
    public ICollection<GroupFile> GroupFiles { get; set; }
    public ICollection<GroupMember> GroupMembers { get; set; }
}
