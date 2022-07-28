namespace Learnz.Entities;
public class CreateQuestionDistributeAnswer
{
    public Guid Id { get; set; }
    public string LeftSide { get; set; }
    public string RightSide { get; set; }
    public Guid QuestionDistributeId { get; set; }
    public CreateQuestionDistribute QuestionDistribute { get; set; }
}
