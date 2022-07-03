using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Learnz.Controllers;

[Route("api/[controller]")]
[ApiController]
[AllowAnonymous]
public class UserRefreshToken : Controller
{
    private readonly DataContext _dataContext;
    private readonly IConfiguration _configuration;
    public UserRefreshToken(DataContext dataContext, IConfiguration configuration)
    {
        _dataContext = dataContext;
        _configuration = configuration;
    }

    [HttpPost]
    public async Task<ActionResult<TokenDTO>> RefreshToken(UserRefreshTokenDTO request)
    {
        var user = await _dataContext.Users.Where(usr => usr.RefreshToken == request.RefreshToken && usr.RefreshExpires >= DateTime.UtcNow).FirstOrDefaultAsync();
        if (user == null)
        {
            return Unauthorized(ErrorKeys.InvalidLogin);
        }

        var token = TokenAuthentication.CreateToken(user.Id, _configuration);

        user.RefreshToken = token.RefreshToken;
        user.RefreshExpires = token.RefreshExpires;
        await _dataContext.SaveChangesAsync();

        return Ok(token);
    }
}
