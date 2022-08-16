namespace Learnz.DTOs;
public class LearnMarkQuestionDTO
{
    public Guid LearnSessionId { get; set; }
    public Guid QuestionId { get; set; }
    public bool Hard { get; set; }
}