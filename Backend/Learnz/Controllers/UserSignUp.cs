using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;

namespace Learnz.Controllers;

[Route("api/[controller]")]
[ApiController]
[AllowAnonymous]
public class UserSignUp : Controller
{
    private readonly DataContext _dataContext;
    private readonly IConfiguration _configuration;
    public UserSignUp(DataContext dataContext, IConfiguration configuration)
    {
        _dataContext = dataContext;
        _configuration = configuration;
    }

    [HttpPost]
    public async Task<ActionResult<TokenDTO>> SignUp(UserSignUpDTO request)
    {
        if (request == null
            || request.Username == "" || request.Password == "" || request.Firstname == "" || request.Lastname == "" || request.Information == "")
        {
            return BadRequest("filloutCorrectly");
        }

        List<Subject> differentSubjects = new List<Subject> { request.GoodSubject1, request.GoodSubject2, request.GoodSubject3, request.BadSubject1, request.BadSubject2, request.BadSubject3 };
        if (differentSubjects.Distinct().ToList().Count != 6)
        {
            return BadRequest("filloutCorrectly");
        }

        if (request.Birthdate > DateTime.UtcNow || request.Birthdate.Year < 1900)
        {
            return BadRequest("filloutCorrectly");
        }

        var usernameTaken = await _dataContext.Users.AnyAsync(usr => usr.Username == request.Username);
        if (usernameTaken)
        {
            return BadRequest("usernameTaken");
        }

        var user = new User
        {
            Id = Guid.NewGuid(),
            Username = request.Username,
            Firstname = request.Firstname,
            Lastname = request.Lastname,
            Birthdate = request.Birthdate,
            Grade = request.Grade,
            ProfileImageId = request.ProfileImageId,
            Information = request.Information,
            GoodSubject1 = request.GoodSubject1,
            GoodSubject2 = request.GoodSubject2,
            GoodSubject3 = request.GoodSubject3,
            BadSubject1 = request.BadSubject1,
            BadSubject2 = request.BadSubject2,
            BadSubject3 = request.BadSubject3
        };

        using var hmac = new HMACSHA512();
        user.PasswordSalt = hmac.Key;
        user.PasswordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(request.Password));

        var token = TokenAuthentication.CreateToken(user.Id, _configuration);

        user.RefreshToken = token.RefreshToken;
        user.RefreshExpires = token.RefreshExpires;

        await _dataContext.Users.AddAsync(user);
        await _dataContext.SaveChangesAsync();

        return Ok(token);
    }
}
