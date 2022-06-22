using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;

namespace Learnz.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class UserProfile : Controller
{
    private readonly DataContext _dataContext;
    private readonly IUserService _userService;
    public UserProfile(DataContext dataContext, IUserService userService)
    {
        _dataContext = dataContext;
        _userService = userService;
    }

    [HttpGet]
    public async Task<ActionResult<UserProfileDTO>> GetProfile()
    {
        var guid = (await _userService.GetUser())!.Id;
        var user = await _dataContext.Users.FirstAsync(u => u.Id == guid);
        var userProfile = new UserProfileDTO
        {
            Username = user.Username,
            Firstname = user.Firstname,
            Lastname = user.Lastname,
            Birthdate = user.Birthdate,
            Grade = user.Grade,
            ProfileImage = user.ProfileImage,
            Information = user.Information,
            GoodSubject1 = user.GoodSubject1,
            GoodSubject2 = user.GoodSubject2,
            GoodSubject3 = user.GoodSubject3,
            BadSubject1 = user.BadSubject1,
            BadSubject2 = user.BadSubject2,
            BadSubject3 = user.BadSubject3
        };
        return Ok(userProfile);
    }

    [HttpPost]
    public async Task<ActionResult> EditProfile(UserProfileDTO request)
    {
        if (request == null
            || request.Username == "" || request.Firstname == "" || request.Lastname == "" || request.Information == "")
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

        var guid = (await _userService.GetUser())!.Id;
        if (await _dataContext.Users.AnyAsync(usr => usr.Username == request.Username && usr.Id != guid))
        {
            return BadRequest("usernameTaken");
        }

        var user = await _dataContext.Users.FirstAsync(u => u.Id == guid);
        user.Username = request.Username;
        user.Firstname = request.Firstname;
        user.Lastname = request.Lastname;
        user.Birthdate = request.Birthdate;
        user.Grade = request.Grade;
        user.ProfileImage = request.ProfileImage;
        user.Information = request.Information;
        user.GoodSubject1 = request.GoodSubject1;
        user.GoodSubject2 = request.GoodSubject2;
        user.GoodSubject3 = request.GoodSubject3;
        user.BadSubject1 = request.BadSubject1;
        user.BadSubject2 = request.BadSubject2;
        user.BadSubject3 = request.BadSubject3;

        await _dataContext.SaveChangesAsync();

        return Ok();
    }

    [HttpPut]
    public async Task<ActionResult> ChangePassword(UserChangePasswordDTO request)
    {
        var guid = (await _userService.GetUser())!.Id;
        var user = await _dataContext.Users.FirstAsync(u => u.Id == guid);

        using var hmacVerify = new HMACSHA512(user.PasswordSalt);
        var computeHash = hmacVerify.ComputeHash(System.Text.Encoding.UTF8.GetBytes(request.OldPassword));
        if (!computeHash.SequenceEqual(user.PasswordHash))
        {
            return BadRequest("passwordNotMatch");
        }

        using var hmacSet = new HMACSHA512();
        user.PasswordSalt = hmacSet.Key;
        user.PasswordHash = hmacSet.ComputeHash(System.Text.Encoding.UTF8.GetBytes(request.NewPassword));

        await _dataContext.SaveChangesAsync();

        return Ok();
    }
}
