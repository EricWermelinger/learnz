namespace Learnz.DTOs;
public class ChallengeAnswerDTO
{
    public Guid ChallengeId { get; set; }
    public Guid QuestionId { get; set; }
    public Guid? AnswerId { get; set; }
    public string? Answer { get; set; }
    public double? AnswerNumeric { get; set; }
    public List<Guid>? Answers { get; set; }
}