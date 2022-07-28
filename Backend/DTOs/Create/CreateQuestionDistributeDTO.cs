namespace Learnz.DTOs;
public class CreateQuestionDistributeDTO
{
    public Guid Id { get; set; }
    public string Question { get; set; }
    public List<CreateQuestionDistributeAnswerDTO> Answers { get; set; }
}