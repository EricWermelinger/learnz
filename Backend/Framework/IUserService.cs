namespace Learnz.Framework;

public interface IUserService
{
    public Task<User> GetUser();
    public Guid GetUserGuid();
    public Task<List<string>> GetConnectionsOfUser(User user);
}
