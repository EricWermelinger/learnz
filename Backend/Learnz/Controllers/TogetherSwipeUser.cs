using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Learnz.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
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
        var alreadySwiped = await _dataContext.TogetherSwipes.Where(swp => swp.SwiperUserId == user.Id || swp.AskedUserId == user.Id)
                                                       .Select(swp => swp.SwiperUserId == user.Id ? swp.AskedUserId : swp.SwiperUserId)
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
        var exists = await _dataContext.TogetherSwipes.AnyAsync(swp => swp.SwiperUserId == guid && swp.AskedUserId == request.UserId && swp.Choice);
        if (!exists)
        {
            await _dataContext.TogetherSwipes.AddAsync(new TogetherSwipe
            {
                Id = Guid.NewGuid(),
                SwiperUserId = guid,
                AskedUserId = request.UserId,
                Choice = request.Choice
            });

            var counts = await _dataContext.TogetherSwipes.Where(swp => (swp.SwiperUserId == guid && swp.AskedUserId == request.UserId && swp.Choice)
                                                                || (swp.SwiperUserId == request.UserId && swp.AskedUserId == guid && swp.Choice))
                                                    .CountAsync();
            if (counts == 2)
            {
                var connectionExists = await _dataContext.TogetherConnections.AnyAsync(cnc =>
                    (cnc.UserId1 == guid && cnc.UserId2 == request.UserId) ||
                    (cnc.UserId1 == request.UserId && cnc.UserId2 == guid));
                if (!connectionExists)
                {
                    await _dataContext.TogetherConnections.AddAsync(new TogetherConnection
                    {
                        Id = Guid.NewGuid(),
                        UserId1 = guid,
                        UserId2 = request.UserId
                    });
                }
            }
        }
        await _dataContext.SaveChangesAsync();
        return Ok();
    }
}
