namespace Learnz.Entities;

public class User
{
    public Guid Id { get; set; }
    public string Username { get; set; }
    public byte[] PasswordHash { get; set; }
    public byte[] PasswordSalt { get; set; }
    public string Firstname { get; set; }
    public string Lastname { get; set; }
    public DateTime Birthdate { get; set; }
    public Grade Grade { get; set; }
    public Guid ProfileImage { get; set; }
    public string Information { get; set; }
    public Subject GoodSubject1 { get; set; }
    public Subject GoodSubject2 { get; set; }
    public Subject GoodSubject3 { get; set; }
    public Subject BadSubject1 { get; set; }
    public Subject BadSubject2 { get; set; }
    public Subject BadSubject3 { get; set; }
    public DateTime? RefreshExpires { get; set; }
    public string? RefreshToken { get; set; }
}
