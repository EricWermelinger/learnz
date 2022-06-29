namespace Learnz.Entities;
public class CreateQuestionMultipleChoice
{
    public Guid Id { get; set; }
    public string Question { get; set; }
    public Guid SetId { get; set; }
    public CreateSet Set { get; set; }

    public ICollection<CreateQuestionMultipleChoiceAnswer> Answers { get; set; }
}
