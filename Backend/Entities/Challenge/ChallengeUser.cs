namespace Learnz.Entities;
public class ChallengeUser
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public User User { get; set; }
    public Guid ChallengeId { get; set; }
    public Challenge Challenge { get; set; }
}