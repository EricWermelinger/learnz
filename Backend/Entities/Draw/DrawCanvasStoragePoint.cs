namespace Learnz.Entities;
public class DrawCanvasStoragePoint
{
    public Guid Id { get; set; }
    public double X { get; set; }
    public double Y { get; set; }

    public ICollection<DrawCanvasStorage> DrawCanvasStoragesFrom { get; set; }
    public ICollection<DrawCanvasStorage> DrawCanvasStoragesTo { get; set; }
}