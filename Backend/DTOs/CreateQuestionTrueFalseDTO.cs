namespace Learnz.DTOs;
public class CreateQuestionTrueFalseDTO
{
    public Guid Id { get; set; }
    public string Question { get; set; }
    public bool Answer { get; set; }
}