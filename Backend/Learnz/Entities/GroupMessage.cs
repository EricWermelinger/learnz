namespace Learnz.Entities;
public class GroupMessage
{
    public Guid Id { get; set; }
    public Guid SenderId { get; set; }
    public User Sender { get; set; }
    public Guid GroupId { get; set; }
    public Group Group { get; set; }
    public string Message { get; set; }
    public DateTime Date { get; set; }
    public bool IsInfoMessage { get; set; }
}
