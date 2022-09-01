namespace Learnz.DTOs;
public class DrawPageGetDTO
{
    public Guid PageId { get; set; }
    public string DataUrl { get; set; }
    public bool Editable { get; set; }
    public bool Deletable { get; set; }
    public string? EditingPersonProfileImagePath { get; set; }
    public string? EditingPersonName { get; set; }
}