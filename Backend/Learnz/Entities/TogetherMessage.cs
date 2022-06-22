namespace Learnz.Entities;
public class TogetherMessage
{
    public Guid Id { get; set; }
    public Guid Sender { get; set; }
    public Guid Receiver { get; set; }
    public string Message { get; set; }
    public DateTime Date { get; set; }
}
