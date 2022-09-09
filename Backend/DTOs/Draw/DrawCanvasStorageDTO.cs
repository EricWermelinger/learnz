namespace Learnz.DTOs;
public class DrawCanvasStorageDTO
{
    public Guid Id { get; set; }
    public DateTime Created { get; set; }
    public DateTime? Deleted { get; set; }
    public string Color { get; set; }
    public DrawCanvasStoragePointDTO FromPosition { get; set; }
    public DrawCanvasStoragePointDTO? ToPosition { get; set; }
    public string Text { get; set; }
}