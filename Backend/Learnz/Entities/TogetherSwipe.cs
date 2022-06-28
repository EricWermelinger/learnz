namespace Learnz.Entities;

public class TogetherSwipe
{
    public Guid Id { get; set; }
    public Guid SwiperUserId { get; set; }
    public User SwiperUser { get; set; }
    public Guid AskedUserId { get; set; }
    public User AskedUser { get; set; }
    public bool Choice { get; set; }
}
