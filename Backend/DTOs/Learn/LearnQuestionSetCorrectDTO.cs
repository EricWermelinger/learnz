namespace Learnz.DTOs;
public class LearnQuestionSetCorrectDTO
{
    public Guid LearnSessionId { get; set; }
    public Guid QuestionId { get; set; }
    public bool Correct { get; set; }
}