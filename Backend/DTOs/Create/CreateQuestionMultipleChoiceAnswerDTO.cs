namespace Learnz.DTOs;
public class CreateQuestionMultipleChoiceAnswerDTO
{
    public Guid Id { get; set; }
    public string Answer { get; set; }
    public bool IsRight { get; set; }
}