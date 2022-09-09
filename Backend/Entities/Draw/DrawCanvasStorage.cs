namespace Learnz.Entities;
public class DrawCanvasStorage
{
    public Guid Id { get; set; }
    public Guid DrawPageId { get; set; }
    public DrawPage DrawPage { get; set; }
    public DateTime Created { get; set; }
    public DateTime? Deleted { get; set; }
    public string Color { get; set; }
    public Guid FromPositionId { get; set; }
    public DrawCanvasStoragePoint FromPosition { get; set; }
    public Guid ToPositionId { get; set; }
    public DrawCanvasStoragePoint ToPosition { get; set; }
    public string Text { get; set; }
}