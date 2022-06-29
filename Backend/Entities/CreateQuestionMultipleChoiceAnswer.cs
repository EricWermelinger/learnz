namespace Learnz.Entities;
public class CreateQuestionMultipleChoiceAnswer
{
    public Guid Id { get; set; }
    public string Answer { get; set; }
    public bool IsRight { get; set; }
    public Guid QuestionMultipleChoiceId { get; set; }
    public CreateQuestionMultipleChoice QuestionMultipleChoice { get; set; }
}
