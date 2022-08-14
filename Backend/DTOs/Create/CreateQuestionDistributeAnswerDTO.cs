namespace Learnz.DTOs;
public class CreateQuestionDistributeAnswerDTO
{
    public Guid Id { get; set; }
    public string LeftSide { get; set; }
    public Guid LeftSideId { get; set; }
    public string RightSide { get; set; }
    public Guid RightSideId { get; set; }
}