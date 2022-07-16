namespace Learnz.DTOs;

public class UserProfileUploadDTO
{
    public string Username { get; set; }
    public string Firstname { get; set; }
    public string Lastname { get; set; }
    public DateTime Birthdate { get; set; }
    public Grade Grade { get; set; }
    public string ProfileImagePath { get; set; }
    public string Information { get; set; }
    public Language Language { get; set; }
    public Subject GoodSubject1 { get; set; }
    public Subject GoodSubject2 { get; set; }
    public Subject GoodSubject3 { get; set; }
    public Subject BadSubject1 { get; set; }
    public Subject BadSubject2 { get; set; }
    public Subject BadSubject3 { get; set; }
    public bool DarkTheme { get; set; }
}
