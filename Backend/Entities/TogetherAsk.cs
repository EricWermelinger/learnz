namespace Learnz.Entities;

public class TogetherAsk
{
    public Guid Id { get; set; }
    public Guid InterestedUserId { get; set; }
    public User InterestedUser { get; set; }
    public Guid AskedUserId { get; set; }
    public User AskedUser { get; set; }
    public bool? Answer { get; set; }
}
