namespace Learnz.DTOs;
public class LearnAnswerDTO
{
    public Guid LearnSessionId { get; set; }
    public Guid QuestionId { get; set; }
    public string Answer { get; set; }
}