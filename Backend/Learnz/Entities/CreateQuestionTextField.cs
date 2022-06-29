namespace Learnz.Entities;
public class CreateQuestionTextField
{
    public Guid Id { get; set; }
    public string Question { get; set; }
    public Guid SetId { get; set; }
    public CreateSet Set { get; set; }
}
