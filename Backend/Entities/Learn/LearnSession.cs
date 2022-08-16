namespace Learnz.Entities;
public class LearnSession
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public User User { get; set; }
    public DateTime Created { get; set; }
    public DateTime? Ended { get; set; }
    public Guid SetId { get; set; }
    public CreateSet Set { get; set; }
    
    public ICollection<LearnQuestion> Questions { get; set; }
}