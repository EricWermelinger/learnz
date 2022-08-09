namespace Learnz.Entities;
public class ChallengeQuestionMathematicResolved
{
    public Guid Id { get; set; }
    public string Question { get; set; }
    public double Answer { get; set; }
    public int Digits { get; set; }
    public Guid ChallengeId { get; set; }
    public Challenge Challenge { get; set; }
}