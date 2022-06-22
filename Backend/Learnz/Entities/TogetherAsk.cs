namespace Learnz.Entities;

public class TogetherAsk
{
    public Guid Id { get; set; }
    public Guid UserId1 { get; set; }
    public Guid UserId2 { get; set; }
    public bool? Answer { get; set; }
}
