namespace Learnz.Entities;

public class WebSocketConnection
{
    public Guid Id { get; set; }
    public string ConnectionId { get; set; }
    public Guid UserId { get; set; }
    public User User { get; set; }
    public DateTime Created { get; set; }
}