using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Learnz.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class TogetherUsersFilter : Controller
{
    private readonly DataContext _dataContext;
    private readonly IUserService _userService;
    private readonly ILearnzFrontendFileGenerator _learnzFrontendFileGenerator;
    public TogetherUsersFilter(DataContext dataContext, IUserService userService, ILearnzFrontendFileGenerator learnzFrontendFileGenerator)
    {
        _dataContext = dataContext;
        _userService = userService;
        _learnzFrontendFileGenerator = learnzFrontendFileGenerator;
    }

    [HttpGet]
    public async Task<ActionResult<List<TogetherUserProfileDTO>>> GetFilteredUsers(string? username, int? grade, int? goodSubject, int? badSubject)
    {
        var guid = _userService.GetUserGuid();
        var connectedUserIds = await _dataContext.TogetherConnections.Where(cnc => cnc.UserId1 == guid || cnc.UserId2 == guid)
                                                                    .Select(cnc => cnc.UserId1 == guid ? cnc.UserId2 : cnc.UserId1)
                                                                    .ToListAsync();
        var users = await _dataContext.Users.Where(usr => usr.Id != guid)
                                      .Where(usr => !connectedUserIds.Contains(usr.Id))
                                      .Where(usr => username == null || username == "" || usr.Username.ToLower().Contains(username.ToLower()))
                                      .Where(usr => grade == null || grade == -1 || (int)usr.Grade == grade)
                                      .Where(usr => goodSubject == null || goodSubject == -1 || (int)usr.GoodSubject1 == goodSubject || (int)usr.GoodSubject2 == goodSubject || (int)usr.GoodSubject3 == goodSubject)
                                      .Where(usr => badSubject == null || badSubject == -1 || (int)usr.BadSubject1 == badSubject || (int)usr.BadSubject2 == badSubject || (int)usr.BadSubject3 == badSubject)
                                      .Select(usr => new
                                      {
                                          User = usr,
                                          ProfileImage = usr.ProfileImage.Path,
                                          TieBreaker = Guid.NewGuid()
                                      })
                                      .OrderBy(usr => usr.TieBreaker)
                                      .Take(10)
                                      .Select(usr => new TogetherUserProfileDTO
                                      {
                                          UserId = usr.User.Id,
                                          Username = usr.User.Username,
                                          ProfileImagePath = _learnzFrontendFileGenerator.PathToImage(usr.ProfileImage),
                                          Grade = usr.User.Grade,
                                          Information = usr.User.Information,
                                          GoodSubject1 = usr.User.GoodSubject1,
                                          GoodSubject2 = usr.User.GoodSubject2,
                                          GoodSubject3 = usr.User.GoodSubject3,
                                          BadSubject1 = usr.User.BadSubject1,
                                          BadSubject2 = usr.User.BadSubject2,
                                          BadSubject3 = usr.User.BadSubject3
                                      })
                                      .ToListAsync();
        return Ok(users);
    }
}
