namespace Learnz.Entities;

public class User
{
    public Guid Id { get; set; }
    public string Username { get; set; }
    public byte[] PasswordHash { get; set; }
    public byte[] PasswordSalt { get; set; }
    public string Firstname { get; set; }
    public string Lastname { get; set; }
    public DateTime Birthdate { get; set; }
    public Grade Grade { get; set; }
    public Guid ProfileImageId { get; set; }
    public LearnzFileAnonymous ProfileImage { get; set; }
    public string Information { get; set; }
    public Language Language { get; set; }
    public Subject GoodSubject1 { get; set; }
    public Subject GoodSubject2 { get; set; }
    public Subject GoodSubject3 { get; set; }
    public Subject BadSubject1 { get; set; }
    public Subject BadSubject2 { get; set; }
    public Subject BadSubject3 { get; set; }
    public DateTime? RefreshExpires { get; set; }
    public string? RefreshToken { get; set; }
    
    public ICollection<Group> GroupAdmin { get; set; }
    public ICollection<GroupMember> GroupMembers { get; set; }
    public ICollection<LearnzFile> LearnzFileCreated { get; set; }
    public ICollection<LearnzFile> LearnzFileModified { get; set; }
    public ICollection<TogetherAsk> TogetherAskInterestedUsers { get; set; }
    public ICollection<TogetherAsk> TogetherAskAskedUsers { get; set; }
    public ICollection<TogetherConnection> TogetherConnectionUsers1 { get; set; }
    public ICollection<TogetherConnection> TogetherConnectionUsers2 { get; set; }
    public ICollection<TogetherMessage> TogetherMessageSenders { get; set; }
    public ICollection<TogetherMessage> TogetherMessageReceivers { get; set; }
    public ICollection<TogetherSwipe> TogetherSwipeSwiperUsers { get; set; }
    public ICollection<TogetherSwipe> TogetherSwipeAskedUsers { get; set; }
    public ICollection<GroupMessage> GroupMessages { get; set; }
    public ICollection<CreateSet> CreateSetCreated { get; set; }
    public ICollection<CreateSet> CreateSetModified { get; set; }
}
