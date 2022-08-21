namespace Learnz.DTOs;
public class LearnSolutionDTO
{
    public Guid QuestionId { get; set; }
    public string Answer { get; set; }
    public bool WasCorrect { get; set; }
}