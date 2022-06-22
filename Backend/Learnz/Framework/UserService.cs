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

    public async Task<User?> GetUser()
    {
        if (_httpContextAccessor.HttpContext == null)
        {
            return null;
        }

        string guid = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Name);

        if (guid == null)
        {
            return null;
        }

        var user = await _context.Users.FindAsync(Guid.Parse(guid));
        return user;
    }

    public Guid? GetUserGuid()
    {
        if (_httpContextAccessor.HttpContext == null)
        {
            return null;
        }

        string guid = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Name);
        return guid != null ? Guid.Parse(guid) : null;
    }
}
