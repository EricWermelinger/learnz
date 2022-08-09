namespace Learnz.Entities;
public class ChallengeQuestionPosed
{
    public Guid Id { get; set; }
    public Guid ChallengeId { get; set; }
    public Challenge Challenge { get; set; }
    public Guid QuestionId { get; set; }
    public string Answer { get; set; }
    public DateTime Created { get; set; }
    public DateTime Expires { get; set; }
    public bool IsActive { get; set; }

    public ICollection<ChallengeQuestionAnswer> ChallengeQuestionAnswers { get; set; }
}