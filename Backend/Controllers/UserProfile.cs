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
    private readonly IFilePolicyChecker _filePolicyChecker;
    private readonly IFileFinder _fileFinder;

    public UserProfile(DataContext dataContext, IUserService userService, IFilePolicyChecker filePolicyChecker, IFileFinder fileFinder)
    {
        _dataContext = dataContext;
        _userService = userService;
        _filePolicyChecker = filePolicyChecker;
        _fileFinder = fileFinder;
    }

    [HttpGet]
    public async Task<ActionResult<UserProfileGetDTO>> GetProfile()
    {
        var guid = _userService.GetUserGuid();
        var user = await _dataContext.Users.Include(u => u.ProfileImage).FirstAsync(u => u.Id == guid);
        var userProfile = new UserProfileGetDTO
        {
            Username = user.Username,
            Firstname = user.Firstname,
            Lastname = user.Lastname,
            Birthdate = user.Birthdate,
            Grade = user.Grade,
            ProfileImagePath = user.ProfileImage.Path,
            ProfileImageName = user.ProfileImage.FileNameExternal,
            Information = user.Information,
            Language = user.Language,
            GoodSubject1 = user.GoodSubject1,
            GoodSubject2 = user.GoodSubject2,
            GoodSubject3 = user.GoodSubject3,
            BadSubject1 = user.BadSubject1,
            BadSubject2 = user.BadSubject2,
            BadSubject3 = user.BadSubject3,
            DarkTheme = user.DarkTheme
        };
        return Ok(userProfile);
    }

    [HttpPost]
    public async Task<ActionResult> EditProfile(UserProfileUploadDTO request)
    {
        if (request == null
            || request.Username == "" || request.Firstname == "" || request.Lastname == "" || request.Information == "")
        {
            return BadRequest(ErrorKeys.FillFormCorrectly);
        }

        List<Subject> differentSubjects = new List<Subject> { request.GoodSubject1, request.GoodSubject2, request.GoodSubject3, request.BadSubject1, request.BadSubject2, request.BadSubject3 };
        if (differentSubjects.Distinct().ToList().Count != 6)
        {
            return BadRequest(ErrorKeys.FillFormCorrectly);
        }

        if (request.Birthdate > DateTime.UtcNow || request.Birthdate.Year < 1900)
        {
            return BadRequest(ErrorKeys.FillFormCorrectly);
        }

        var guid = _userService.GetUserGuid();
        if (await _dataContext.Users.AnyAsync(usr => usr.Username == request.Username && usr.Id != guid))
        {
            return BadRequest(ErrorKeys.UsernameTaken);
        }

        var profileImageId =
            await _fileFinder.GetFileId(_dataContext, guid, request.ProfileImagePath, _filePolicyChecker);
        if (profileImageId == null)
        {
            return BadRequest(ErrorKeys.FileNotValid);
        }

        var user = await _dataContext.Users.FirstAsync(u => u.Id == guid);
        user.Username = request.Username;
        user.Firstname = request.Firstname;
        user.Lastname = request.Lastname;
        user.Birthdate = request.Birthdate;
        user.Grade = request.Grade;
        user.ProfileImageId = (Guid)profileImageId;
        user.Information = request.Information;
        user.Language = request.Language;
        user.GoodSubject1 = request.GoodSubject1;
        user.GoodSubject2 = request.GoodSubject2;
        user.GoodSubject3 = request.GoodSubject3;
        user.BadSubject1 = request.BadSubject1;
        user.BadSubject2 = request.BadSubject2;
        user.BadSubject3 = request.BadSubject3;
        user.DarkTheme = request.DarkTheme;

        await _dataContext.SaveChangesAsync();

        return Ok();
    }

    [HttpPut]
    public async Task<ActionResult> ChangePassword(UserChangePasswordDTO request)
    {
        var guid = _userService.GetUserGuid();
        var user = await _dataContext.Users.FirstAsync(u => u.Id == guid);

        using var hmacVerify = new HMACSHA512(user.PasswordSalt);
        var computeHash = hmacVerify.ComputeHash(System.Text.Encoding.UTF8.GetBytes(request.OldPassword));
        if (!computeHash.SequenceEqual(user.PasswordHash))
        {
            return BadRequest(ErrorKeys.PasswordNotMatch);
        }

        using var hmacSet = new HMACSHA512();
        user.PasswordSalt = hmacSet.Key;
        user.PasswordHash = hmacSet.ComputeHash(System.Text.Encoding.UTF8.GetBytes(request.NewPassword));

        await _dataContext.SaveChangesAsync();

        return Ok();
    }
}
