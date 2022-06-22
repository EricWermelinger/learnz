namespace Learnz.DTOs;

public class TogetherUserProfileDTO
{
    public Guid UserId { get; set; }
    public string Username { get; set; }
    public Grade Grade { get; set; }
    public Guid ProfileImage { get; set; }
    public string Information { get; set; }
    public Subject GoodSubject1 { get; set; }
    public Subject GoodSubject2 { get; set; }
    public Subject GoodSubject3 { get; set; }
    public Subject BadSubject1 { get; set; }
    public Subject BadSubject2 { get; set; }
    public Subject BadSubject3 { get; set; }
}
