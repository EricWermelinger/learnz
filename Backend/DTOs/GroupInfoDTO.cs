namespace Learnz.DTOs;
public class GroupInfoDTO
{
    public Guid GroupId { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public LearnzFileFrontendDTO ProfileImage { get; set; }
    public List<GroupInfoMemberDTO> Members { get; set; }
    public bool IsUserAdmin { get; set; }
}
