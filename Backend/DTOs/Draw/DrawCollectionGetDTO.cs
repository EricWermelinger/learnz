namespace Learnz.DTOs;
public class DrawCollectionGetDTO
{
    public Guid CollectionId { get; set; }
    public string Name { get; set; }
    public int NumberOfPages { get; set; }
    public Guid FirstPageId { get; set; }
    public bool Editable { get; set; }
    public bool Deletable { get; set; }
    public bool IsGroupCollection { get; set; }
    public string? GroupName { get; set; }
    public DateTime LastChanged { get; set; }
    public string LastChangedBy { get; set; }
}