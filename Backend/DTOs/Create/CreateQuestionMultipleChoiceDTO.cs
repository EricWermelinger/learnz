namespace Learnz.DTOs;
public class CreateQuestionMultipleChoiceDTO
{
    public Guid Id { get; set; }
    public string Question { get; set; }
    public List<CreateQuestionMultipleChoiceAnswerDTO> Answers { get; set; }
}