namespace Learnz.DTOs;
public class DrawCollectionUpsertDTO
{
    public Guid CollectionId { get; set; }
    public Guid? FirstPageId { get; set; }
    public string Name { get; set; }
    public Guid? GroupId { get; set; }
    public DrawGroupPolicy? DrawGroupPolicy { get; set; }
}