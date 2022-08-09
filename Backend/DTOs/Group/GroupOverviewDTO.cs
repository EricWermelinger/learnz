namespace Learnz.DTOs;
public class GroupOverviewDTO
{
    public Guid GroupId { get; set; }
    public string GroupName { get; set; }
    public string ProfileImagePath { get; set; }
    public bool? LastMessageSentByMe { get; set; }
    public string? LastMessage { get; set; }
    public DateTime? LastMessageDateSent { get; set; }
    public string? LastMessageSentUsername { get; set; }
    public bool? LastMessageWasInfoMessage { get; set; }
    public int NumberOfFiles { get; set; }
}
