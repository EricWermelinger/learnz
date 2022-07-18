namespace Learnz.DTOs;
public class GroupInfoDTO
{
    public Guid GroupId { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string ProfileImagePath { get; set; }
    public List<GroupInfoMemberDTO> Members { get; set; }
    public bool IsUserAdmin { get; set; }
}
