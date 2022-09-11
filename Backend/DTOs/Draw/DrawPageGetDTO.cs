namespace Learnz.DTOs;
public class DrawPageGetDTO
{
    public Guid PageId { get; set; }
    public string DataUrl { get; set; }
    public bool Editable { get; set; }
    public bool Deletable { get; set; }
    public string? EditingPersonProfileImagePath { get; set; }
    public string? EditingPersonName { get; set; }
    public bool IsEmpty { get; set; }
}

public class DrawPageGetBackendDTO
{
    public Guid PageId { get; set; }
    public string DataUrl { get; set; }
    public bool Editable { get; set; }
    public bool Deletable { get; set; }
    public Guid? EditingPersonId { get; set; }
    public string? EditingPersonProfileImagePath { get; set; }
    public string? EditingPersonName { get; set; }
    public Guid OwnerId { get; set; }
    public DrawGroupPolicy Policy { get; set; }
    public int PageCount { get; set; }
    public bool IsEmpty { get; set; }
}