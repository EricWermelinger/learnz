namespace Learnz.DTOs;
public class ChallengeAnswerDTO
{
    public Guid ChallengeId { get; set; }
    public Guid QuestionId { get; set; }
    public string Answer { get; set; }
}