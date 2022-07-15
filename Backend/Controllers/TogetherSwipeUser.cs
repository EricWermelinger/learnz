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
    private readonly IPathToImageConverter _pathToImageConverter;
    public TogetherSwipeUser(DataContext dataContext, IUserService userService, IPathToImageConverter pathToImageConverter)
    {
        _dataContext = dataContext;
        _userService = userService;
        _pathToImageConverter = pathToImageConverter;
    }

    [HttpGet]
    public async Task<ActionResult<TogetherUserProfileDTO>> GetNextSwipe()
    {
        var user = await _userService.GetUser();
        var alreadySwiped = await _dataContext.TogetherSwipes.Where(swp => swp.SwiperUserId == user.Id)
                                                       .Select(swp => swp.AskedUserId)
                                                       .ToListAsync();
        var openSwipes = await _dataContext.TogetherSwipes.Where(swp => swp.AskedUserId == user.Id && swp.Choice)
                                                        .Select(swp => swp.SwiperUserId)
                                                        .Where(swp => !alreadySwiped.Contains(swp))
                                                        .ToListAsync();
        var alreadyConnection = await _dataContext.TogetherConnections
            .Where(cnc => cnc.UserId1 == user.Id || cnc.UserId2 == user.Id)
            .Select(cnc => cnc.UserId1 == user.Id ? cnc.UserId2 : cnc.UserId1)
            .ToListAsync();

        var nextPossibilities = await _dataContext.Users.Where(usr => !alreadySwiped.Contains(usr.Id) && !alreadyConnection.Contains(usr.Id))
                                          .Where(usr => usr.Id != user.Id)
                                          .Select(usr => new
                                          {
                                              User = usr,
                                              ProfileImage = usr.ProfileImage.Path,
                                              ScoreSubject = CalculateSubjectScore(user, usr),
                                              ScoreGrade = CalculateGradeScore(user.Grade, usr.Grade),
                                              TieBreaker = Guid.NewGuid(),
                                              OpenSwipe = openSwipes.Contains(usr.Id) ? 1 : 0,
                                          })
                                          .OrderByDescending(usr => usr.OpenSwipe)
                                          .ThenByDescending(usr => usr.TieBreaker)
                                          .Take(10)
                                          .ToListAsync();

        var nextSwipe = nextPossibilities.OrderByDescending(usr => usr.OpenSwipe)
                                          .ThenBy(usr => (usr.ScoreSubject + usr.ScoreGrade))
                                          .ThenBy(usr => usr.ScoreGrade)
                                          .ThenBy(usr => usr.TieBreaker)
                                          .Select(usr => new TogetherUserProfileDTO
                                          {
                                              UserId = usr.User.Id,
                                              Username = usr.User.Username,
                                              Grade = usr.User.Grade,
                                              ProfileImagePath = _pathToImageConverter.PathToImage(usr.ProfileImage),
                                              Information = usr.User.Information,
                                              GoodSubject1 = usr.User.GoodSubject1,
                                              GoodSubject2 = usr.User.GoodSubject2,
                                              GoodSubject3 = usr.User.GoodSubject3,
                                              BadSubject1 = usr.User.BadSubject1,
                                              BadSubject2 = usr.User.BadSubject2,
                                              BadSubject3 = usr.User.BadSubject3
                                          })
                                          .FirstOrDefault();
        if (nextSwipe == null)
        {
            return BadRequest(ErrorKeys.TogetherNoUserFound);
        }
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

            await _dataContext.SaveChangesAsync();

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

    private static int CalculateSubjectScore(User userA, User userB)
    {
        int score = 0;
        var subjects = (Subject[])Enum.GetValues(typeof(Subject));
        foreach (Subject sbj in subjects)
        {
            int subjectScore = 0;
            subjectScore += userA.GoodSubject1 == sbj ? 3 : 0;
            subjectScore += userA.GoodSubject2 == sbj ? 2 : 0;
            subjectScore += userA.GoodSubject3 == sbj ? 1 : 0;
            subjectScore += userB.GoodSubject1 == sbj ? 3 : 0;
            subjectScore += userB.GoodSubject2 == sbj ? 2 : 0;
            subjectScore += userB.GoodSubject3 == sbj ? 1 : 0;
            subjectScore += userA.BadSubject1 == sbj ? -3 : 0;
            subjectScore += userA.BadSubject2 == sbj ? -2 : 0;
            subjectScore += userA.BadSubject3 == sbj ? -1 : 0;
            subjectScore += userB.BadSubject1 == sbj ? -3 : 0;
            subjectScore += userB.BadSubject2 == sbj ? -2 : 0;
            subjectScore += userB.BadSubject3 == sbj ? -1 : 0;
            score += Math.Abs(subjectScore);
        }
        return score;
    }

    private static int CalculateGradeScore(Grade gradeA, Grade gradeB)
    {
        int a = (int)gradeA;
        int b = (int)gradeB;
        return Math.Abs(a - b);
    }
}
