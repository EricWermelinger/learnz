using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Learnz.Controllers;

[Route("api/[controller]")]
[ApiController]
[AllowAnonymous]
public class TogetherSwipeUser : Controller
{
    private readonly DataContext _dataContext;
    private readonly IUserService _userService;
    public TogetherSwipeUser(DataContext dataContext, IUserService userService)
    {
        _dataContext = dataContext;
        _userService = userService;
    }

    [HttpGet]
    public async Task<ActionResult<TogetherUserProfileDTO?>> GetNextSwipe()
    {
        var user = await _userService.GetUser();
        var alreadySwiped = await _dataContext.TogetherSwipes.Where(swp => swp.UserId1 == user.Id || swp.UserId2 == user.Id)
                                                       .Select(swp => swp.UserId1 == user.Id ? swp.UserId2 : swp.UserId1)
                                                       .ToListAsync();
        var nextSwipe = await _dataContext.Users.Where(usr => !alreadySwiped.Contains(usr.Id))
                                          .Select(usr => new
                                          {
                                              User = usr,
                                              ScoreSubject = Enum.GetValues(typeof(Subject)).Cast<Subject>().Select(sbj =>
                                                            user.GoodSubject1 == sbj ? 3 : 0
                                                            + user.GoodSubject2 == sbj ? 2 : 0
                                                            + user.GoodSubject3 == sbj ? 1 : 0
                                                            + usr.GoodSubject1 == sbj ? 3 : 0
                                                            + usr.GoodSubject2 == sbj ? 2 : 0
                                                            + usr.GoodSubject3 == sbj ? 1 : 0
                                                            + user.BadSubject1 == sbj ? -3 : 0
                                                            + user.BadSubject2 == sbj ? -2 : 0
                                                            + user.BadSubject3 == sbj ? -1 : 0
                                                            + usr.BadSubject1 == sbj ? -3 : 0
                                                            + usr.BadSubject2 == sbj ? -2 : 0
                                                            + usr.BadSubject3 == sbj ? -1 : 0
                                                        )
                                                .Sum(scr => scr),
                                              ScoreGrade = Math.Abs((int)usr.Grade - (int)usr.Grade),
                                              TieBreaker = new Random().Next(100)
                                          })
                                          .OrderBy(usr => (usr.ScoreSubject + usr.ScoreGrade))
                                          .ThenBy(usr => usr.ScoreGrade)
                                          .ThenBy(usr => usr.TieBreaker)
                                          .Select(usr => new TogetherUserProfileDTO
                                          {
                                              UserId = usr.User.Id,
                                              Username = usr.User.Username,
                                              Grade = usr.User.Grade,
                                              ProfileImage = usr.User.ProfileImage,
                                              Information = usr.User.Information,
                                              GoodSubject1 = usr.User.GoodSubject1,
                                              GoodSubject2 = usr.User.GoodSubject2,
                                              GoodSubject3 = usr.User.GoodSubject3,
                                              BadSubject1 = usr.User.BadSubject1,
                                              BadSubject2 = usr.User.BadSubject2,
                                              BadSubject3 = usr.User.BadSubject3
                                          })
                                          .FirstOrDefaultAsync();
        return Ok(nextSwipe);
    }

    [HttpPost]
    public async Task<ActionResult> Swipe(TogetherSwipeDTO request)
    {
        var guid = _userService.GetUserGuid();
        var exists = await _dataContext.TogetherSwipes.AnyAsync(swp => (swp.UserId1 == guid && swp.UserId2 == request.UserId && swp.Choice));
        if (!exists)
        {
            _dataContext.TogetherSwipes.Add(new TogetherSwipe
            {
                Id = Guid.NewGuid(),
                UserId1 = guid ?? Guid.NewGuid(),
                UserId2 = request.UserId,
                Choice = request.Choice
            });

            var counts = await _dataContext.TogetherSwipes.Where(swp => (swp.UserId1 == guid && swp.UserId2 == request.UserId && swp.Choice)
                                                                || (swp.UserId1 == request.UserId && swp.UserId2 == guid && swp.Choice))
                                                    .CountAsync();
            if (counts == 2)
            {
                _dataContext.TogetherConnections.Add(new TogetherConnection
                {
                    Id = Guid.NewGuid(),
                    UserId1 = guid ?? Guid.NewGuid(),
                    UserId2 = request.UserId
                });
            }
        }
        await _dataContext.SaveChangesAsync();
        return Ok();
    }
}
