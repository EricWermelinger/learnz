namespace Learnz.Entities;
public class CreateQuestionOpenQuestion
{
    public Guid Id { get; set; }
    public string Question { get; set; }
    public string Answer { get; set; }
    public Guid SetId { get; set; }
    public CreateSet Set { get; set; }
}
