namespace Learnz.Entities;
public class DrawGroupCollection
{
    public Guid Id { get; set; }
    public Guid DrawCollectionId { get; set; }
    public DrawCollection DrawCollection { get; set; }
    public Guid GroupId { get; set; }
    public Group Group { get; set; }
    public DrawGroupPolicy DrawGroupPolicy { get; set; }
}