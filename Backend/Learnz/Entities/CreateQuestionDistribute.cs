namespace Learnz.Entities;
public class CreateQuestionDistribute
{
    public Guid Id { get; set; }
    public string Question { get; set; }
    public Guid SetId { get; set; }
    public CreateSet Set { get; set; }
    
    public ICollection<CreateQuestionDistributeAnswer> Answers { get; set; }
}
