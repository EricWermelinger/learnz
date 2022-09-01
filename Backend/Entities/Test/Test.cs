namespace Learnz.Entities;
public class Test
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public Guid SetId { get; set; }
    public CreateSet Set { get; set; }
    public Guid OwnerId { get; set; }
    public User Owner { get; set; }
    public DateTime Created { get; set; }
    public int MaxTime { get; set; }
    public bool Visible { get; set; }
    public bool Active { get; set; }

    public ICollection<TestGroup> TestGroups { get; set; }
    public ICollection<TestQuestion> TestQuestions { get; set; }
    public ICollection<TestOfUser> TestOfUsers { get; set; }
}