namespace Learnz.Entities;
public class Challenge
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public Guid CreateSetId { get; set; }
    public CreateSet CreateSet { get; set; }
    public ChallengeState State { get; set; }
    public Guid OwnerId { get; set; }
    public User Owner { get; set; }

    public ICollection<ChallengeQuestionAnswer> ChallengeQuestionAnswers { get; set; }
    public ICollection<ChallengeUser> ChallengeUsers { get; set; }
    public ICollection<ChallengeQuestionMathematicResolved> ChallengeQuestionMathematicResolveds { get; set; }
    public ICollection<ChallengeQuestionPosed> ChallengeQuestionsPosed { get; set; }
}
