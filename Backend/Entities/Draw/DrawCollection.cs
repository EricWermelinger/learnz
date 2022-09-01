namespace Learnz.Entities;
public class DrawCollection
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public Guid OwnerId { get; set; }
    public User Owner { get; set; }
    public DateTime Changed { get; set; }
    public Guid ChangedById { get; set; }
    public User ChangedBy { get; set; }

    public ICollection<DrawPage> DrawPages { get; set; }
    public ICollection<DrawGroupCollection> DrawGroupCollections { get; set; }
}