using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;

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
    public async Task<ActionResult<UserTokenDTO>> RefreshToken(UserRefreshTokenDTO request)
    {
        var user = await _dataContext.Users.Where(usr => usr.RefreshToken == request.RefreshToken && usr.RefreshExpires >= DateTime.UtcNow).FirstOrDefaultAsync();
        if (user == null)
        {
            return BadRequest(ErrorKeys.InvalidLogin);
        }

        var token = TokenAuthentication.GenerateToken(user.Id, _configuration);
        var refresh = new UserTokenDTO
        {
            Token = new JwtSecurityTokenHandler().WriteToken(token)
        };
        return Ok(refresh);
    }
}
