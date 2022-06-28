namespace Learnz.DTOs;
public class GroupInfoMemberDTO
{
    public Guid UserId { get; set; }
    public string Username { get; set; }
    public string Firstname { get; set; }
    public string Lastname { get; set; }
    public bool IsAdmin { get; set; }
    public string ProfileImagePath { get; set; }
}
