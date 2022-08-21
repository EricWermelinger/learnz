namespace Learnz.Entities;
public class TestOfUser
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public User User { get; set; }
    public Guid TestId { get; set; }
    public Test Test { get; set; }
    public DateTime Started { get; set; }
    public DateTime? Ended { get; set; }

    public ICollection<TestQuestionOfUser> TestQuestionOfUsers { get; set; }
}