namespace Learnz.Entities;
public class Group
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public Guid ProfileImageId { get; set; }
    public LearnzFile ProfileImage { get; set; }
    public Guid AdminId { get; set; }
    public User Admin { get; set; }
   
    public ICollection<GroupFile> GroupFiles { get; set; }
    public ICollection<GroupMember> GroupMembers { get; set; }
    public IEnumerable<GroupMessage> GroupMessages { get; set; }
    public ICollection<TestGroup> TestGroups { get; set; }
    public ICollection<DrawGroupCollection> DrawGroupCollections { get; set; }
}
