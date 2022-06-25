namespace Learnz.Entities;
public class TogetherMessage
{
    public Guid Id { get; set; }
    public Guid SenderId { get; set; }
    public User Sender { get; set; }
    public Guid ReceiverId { get; set; }
    public User Receiver { get; set; }
    public string Message { get; set; }
    public DateTime Date { get; set; }
}
