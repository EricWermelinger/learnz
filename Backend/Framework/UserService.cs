using System.Security.Claims;

namespace Learnz.Framework;

public class UserService : IUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly DataContext _context;

    public UserService(IHttpContextAccessor httpContextAccessor, DataContext context)
    {
        _httpContextAccessor = httpContextAccessor;
        _context = context;
    }
    
    public async Task<User> GetUser()
    {
        string guid = _httpContextAccessor.HttpContext!.User.FindFirstValue(ClaimTypes.Name);
        var user = await _context.Users.FindAsync(Guid.Parse(guid));
        return user;
    }

    public Guid GetUserGuid()
    {
        string guid = _httpContextAccessor.HttpContext!.User.FindFirstValue(ClaimTypes.Name);
        return Guid.Parse(guid);
    }
    
    public async Task<List<string>> GetConnectionsOfUser(User user)
    {
        var connections = await _context.WebSocketConnections.Where(cnc => cnc.UserId == user.Id)
                                                             .Select(cnc => cnc.ConnectionId)
                                                             .ToListAsync();
        return connections;
    }
}
