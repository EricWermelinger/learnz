namespace Learnz.DTOs;
public class DrawPageEditDTO
{
    public Guid CollectionId { get; set; }
    public Guid PageId { get; set; }
    public string DataUrl { get; set; }
    public List<DrawCanvasStorageDTO> CanvasStorage { get; set; }
    public DateTime StepperPosition { get; set; }
}