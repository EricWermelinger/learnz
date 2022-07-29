namespace Learnz.Entities;
public class ChallengeQuestionAnswer
{
    public Guid Id { get; set; }
    public Guid ChallengeId { get; set; }
    public Challenge Challenge { get; set; }
    public Guid ChallengeQuestionPosedId { get; set; }
    public ChallengeQuestionPosed ChallengeQuestionPosed { get; set; }
    public Guid UserId { get; set; }
    public User User { get; set; }
    public string Answer { get; set; }
    public bool IsRight { get; set; }
    public int Points { get; set; }
}