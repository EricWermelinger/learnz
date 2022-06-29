using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;

namespace Learnz.Controllers;

[Route("api/[controller]")]
[ApiController]
[AllowAnonymous]
public class UserLogin : Controller
{
    private readonly DataContext _dataContext;
    private readonly IConfiguration _configuration;
    public UserLogin(DataContext dataContext, IConfiguration configuration)
    {
        _dataContext = dataContext;
        _configuration = configuration;
    }

    [HttpPost]
    public async Task<ActionResult<TokenDTO>> LoginUser(UserLoginDTO request)
    {
        if (request == null)
        {
            return BadRequest();
        }

        var user = await _dataContext.Users.Where(usr => usr.Username == request.Username).FirstOrDefaultAsync();
        if (user == null)
        {
            return Unauthorized("invalidLogin");
        }
        using var hmac = new HMACSHA512(user.PasswordSalt);
        var computeHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(request.Password));
        if (!computeHash.SequenceEqual(user.PasswordHash))
        {
            return Unauthorized("invalidLogin");
        }

        var token = TokenAuthentication.CreateToken(user.Id, _configuration);

        user.RefreshToken = token.RefreshToken;
        user.RefreshExpires = token.RefreshExpires;
        await _dataContext.SaveChangesAsync();

        return Ok(token);
    }
}
