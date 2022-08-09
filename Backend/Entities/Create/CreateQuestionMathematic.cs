namespace Learnz.Entities;
public class CreateQuestionMathematic
{
    public Guid Id { get; set; }
    public string Question { get; set; }
    public string Answer { get; set; }
    public int Digits { get; set; }
    public Guid SetId { get; set; }
    public CreateSet Set { get; set; }

    public ICollection<CreateQuestionMathematicVariable> Variables { get; set; }
}
