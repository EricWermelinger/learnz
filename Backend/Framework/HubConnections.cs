namespace Learnz.Framework;
public static class HubConnections
{
    private static List<WebSocketConnection> Connections = new List<WebSocketConnection>();

    public static void AddConnection(string connectionId)
    {
        if (connectionId != null)
        {
            Connections.Add(new WebSocketConnection
            {
                ConnectionId = connectionId
            });
        }
    }

    public static void EditConnection(string connectionId, Guid userId)
    {
        var edit = Connections.FirstOrDefault(cnc => cnc.ConnectionId == connectionId);
        if (edit != null)
        {
            edit.UserId = userId;
        }
    }

    public static void DeleteConnection(string connectionId)
    {
        var remove = Connections.FirstOrDefault(cnc => cnc.ConnectionId == connectionId);
        if (remove != null)
        {
            Connections.Remove(remove);
        }
    }

    public static List<string> ConnectionsOfUser(Guid userId)
    {
        return Connections.Where(cnc => cnc.UserId == userId && cnc.UserId != null).Select(cnc => cnc.ConnectionId).ToList();
    }
}
