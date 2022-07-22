namespace Learnz.Services;
public interface IUserService
{
    public Task<User> GetUser();
    public Guid GetUserGuid();
}
