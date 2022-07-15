using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Learnz.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class TogetherUsers : Controller
{
    private readonly DataContext _dataContext;
    private readonly IUserService _userService;
    private readonly IPathToImageConverter _pathToImageConverter;
    public TogetherUsers(DataContext dataContext, IUserService userService, IPathToImageConverter pathToImageConverter)
    {
        _dataContext = dataContext;
        _userService = userService;
        _pathToImageConverter = pathToImageConverter;
    }

    [HttpGet]
    public async Task<ActionResult<List<TogetherUserProfileDTO>>> GetAllUsers()
    {
        var guid = _userService.GetUserGuid();
        var users = await _dataContext.Users.Where(usr => usr.Id != guid)
                                      .Select(usr => new TogetherUserProfileDTO
                                      {
                                          UserId = usr.Id,
                                          Username = usr.Username,
                                          ProfileImagePath = _pathToImageConverter.PathToImage(usr.ProfileImage.Path),
                                          Grade = usr.Grade,
                                          Information = usr.Information,
                                          GoodSubject1 = usr.GoodSubject1,
                                          GoodSubject2 = usr.GoodSubject2,
                                          GoodSubject3 = usr.GoodSubject3,
                                          BadSubject1 = usr.BadSubject1,
                                          BadSubject2 = usr.BadSubject2,
                                          BadSubject3 = usr.BadSubject3
                                      })
                                      .ToListAsync();
        return Ok(users);
    }
}
