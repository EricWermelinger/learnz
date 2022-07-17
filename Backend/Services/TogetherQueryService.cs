namespace Learnz.Framework;
public class TogetherQueryService : ITogetherQueryService
{
    private readonly DataContext _dataContext;
    private readonly IPathToImageConverter _pathToImageConverter;
    public TogetherQueryService(DataContext dataContext, IPathToImageConverter pathToImageConverter)
    {
        _dataContext = dataContext;
        _pathToImageConverter = pathToImageConverter;
    }

    public async Task<TogetherAskOverviewDTO> GetOpenAsks(Guid guid)
    {
        var connectedUserIds = await _dataContext.TogetherConnections.Where(cnc => cnc.UserId1 == guid || cnc.UserId2 == guid)
                                                                    .Select(cnc => cnc.UserId1 == guid ? cnc.UserId2 : cnc.UserId1)                                                            
                                                                    .ToListAsync();

        var openAsks = await _dataContext.TogetherAsks.Where(ask => ask.Answer == null && ask.AskedUserId == guid)
                                                     .Where(ask => !connectedUserIds.Contains(ask.InterestedUserId))
                                                     .Select(ask => ask.InterestedUser)
                                                     .Select(usr => new TogetherUserProfileDTO
                                                     {
                                                         UserId = usr.Id,
                                                         Username = usr.Username,
                                                         Grade = usr.Grade,
                                                         ProfileImagePath = _pathToImageConverter.PathToImage(usr.ProfileImage.Path),
                                                         Information = usr.Information,
                                                         GoodSubject1 = usr.GoodSubject1,
                                                         GoodSubject2 = usr.GoodSubject2,
                                                         GoodSubject3 = usr.GoodSubject3,
                                                         BadSubject1 = usr.BadSubject1,
                                                         BadSubject2 = usr.BadSubject2,
                                                         BadSubject3 = usr.BadSubject3
                                                     })
                                                     .ToListAsync();

        var sentAsks = await _dataContext.TogetherAsks.Where(ask => ask.Answer == null && ask.InterestedUserId == guid)
                                                     .Where(ask => !connectedUserIds.Contains(ask.AskedUserId))
                                                     .Select(ask => ask.AskedUser)
                                                     .Select(usr => new TogetherUserProfileDTO
                                                     {
                                                         UserId = usr.Id,
                                                         Username = usr.Username,
                                                         Grade = usr.Grade,
                                                         ProfileImagePath = _pathToImageConverter.PathToImage(usr.ProfileImage.Path),
                                                         Information = usr.Information,
                                                         GoodSubject1 = usr.GoodSubject1,
                                                         GoodSubject2 = usr.GoodSubject2,
                                                         GoodSubject3 = usr.GoodSubject3,
                                                         BadSubject1 = usr.BadSubject1,
                                                         BadSubject2 = usr.BadSubject2,
                                                         BadSubject3 = usr.BadSubject3
                                                     })
                                                     .ToListAsync();

        return new TogetherAskOverviewDTO
        {
            OpenAsks = openAsks,
            SentAsks = sentAsks
        };
    }

    public async Task<List<TogetherOverviewUserProfileDTO>> GetConnectionOverview(Guid guid)
    {
        var users = await _dataContext.TogetherConnections.Where(cnc => cnc.UserId1 == guid || cnc.UserId2 == guid)
                                                               .Include(usr => usr.User1.ProfileImage)
                                                               .Include(usr => usr.User2.ProfileImage)
                                                               .Select(cnc => cnc.UserId1 == guid ? cnc.User2 : cnc.User1)
                                                               .Select(usr => new TogetherOverviewUserProfileDTO
                                                               {
                                                                   UserId = usr.Id,
                                                                   Username = usr.Username,
                                                                   Grade = usr.Grade,
                                                                   ProfileImagePath = _pathToImageConverter.PathToImage(usr.ProfileImage.Path),
                                                                   Information = usr.Information,
                                                                   GoodSubject1 = usr.GoodSubject1,
                                                                   GoodSubject2 = usr.GoodSubject2,
                                                                   GoodSubject3 = usr.GoodSubject3,
                                                                   BadSubject1 = usr.BadSubject1,
                                                                   BadSubject2 = usr.BadSubject2,
                                                                   BadSubject3 = usr.BadSubject3
                                                               })
                                                               .ToListAsync();

        var messages = await _dataContext.TogetherMessages.Where(msg => msg.SenderId == guid || msg.ReceiverId == guid)
            .ToListAsync();

        foreach (var user in users)
        {
            var message = messages.Where(msg => msg.SenderId == user.UserId || msg.ReceiverId == user.UserId)
                .OrderByDescending(msg => msg.Date)
                .FirstOrDefault();
            if (message != null)
            {
                user.LastMessage = message.Message;
                user.LastMessageDateSent = message.Date;
                user.LastMessageSentByMe = message.SenderId == guid;
            }
        }

        return users.OrderByDescending(usr => usr.LastMessageDateSent).ToList();
    }

    public async Task<TogetherChatDTO> GetMessages(Guid chatFrom, Guid chatTo)
    {
        var messages = await _dataContext.TogetherMessages.Where(msg => (msg.SenderId == chatFrom && msg.ReceiverId == chatTo) || (msg.ReceiverId == chatFrom && msg.SenderId == chatTo))
            .Select(msg => new TogetherChatMessageDTO
            {
                Message = msg.Message,
                DateSent = msg.Date,
                SentByMe = msg.SenderId == chatFrom
            })
            .OrderByDescending(msg => msg.DateSent)
            .ToListAsync();
        var user = await _dataContext.Users.Select(usr => new TogetherUserProfileDTO
                                                        {
                                                            UserId = usr.Id,
                                                            Username = usr.Username,
                                                            Grade = usr.Grade,
                                                            Information = usr.Information,
                                                            ProfileImagePath = _pathToImageConverter.PathToImage(usr.ProfileImage.Path),
                                                            GoodSubject1 = usr.GoodSubject1,
                                                            GoodSubject2 = usr.GoodSubject2,
                                                            GoodSubject3 = usr.GoodSubject3,
                                                            BadSubject1 = usr.BadSubject1,
                                                            BadSubject2 = usr.BadSubject2,
                                                            BadSubject3 = usr.BadSubject3,
                                                        })
                                               .FirstAsync(usr => usr.UserId == chatTo);
        return new TogetherChatDTO
        {
            User = user,
            Messages = messages
        };
    }
}
