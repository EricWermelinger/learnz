namespace Learnz.Entities;
public class DrawPage
{
    public Guid Id { get; set; }
    public Guid DrawCollectionId { get; set; }
    public DrawCollection DrawCollection { get; set; }
    public Guid OwnerId { get; set; }
    public User Owner { get; set; }
    public DateTime Created { get; set; }
    public DateTime Changed { get; set; }
    public Guid ChangedById { get; set; }
    public User ChangedBy { get; set; }
    public string DataUrl { get; set; }
}