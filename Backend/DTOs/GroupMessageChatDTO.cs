namespace Learnz.DTOs;
public class GroupMessageChatDTO
{
    public string GroupName { get; set; }
    public string ProfileImagePath { get; set; }
    public List<GroupMessageGetDTO> Messages { get; set; }
}
