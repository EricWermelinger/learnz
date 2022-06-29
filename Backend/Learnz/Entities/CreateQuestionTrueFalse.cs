namespace Learnz.Entities;
public class CreateQuestionTrueFalse
{
    public Guid Id { get; set; }
    public string Question { get; set; }
    public bool Answer { get; set; }
    public Guid SetId { get; set; }
    public CreateSet Set { get; set; }
}
