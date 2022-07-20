namespace Learnz.Services;
public class GroupQueryService : IGroupQueryService
{
    private readonly DataContext _dataContext;
    private readonly ILearnzFrontendFileGenerator _learnzFrontendFileGenerator;
    private readonly IFilePolicyChecker _filePolicyChecker;
    public GroupQueryService(DataContext dataContext, ILearnzFrontendFileGenerator learnzFrontendFileGenerator, IFilePolicyChecker filePolicyChecker)
    {
        _dataContext = dataContext;
        _learnzFrontendFileGenerator = learnzFrontendFileGenerator;
        _filePolicyChecker = filePolicyChecker;
    }

    public async Task<List<FileFrontendHistorizedDTO>> GetFiles(Guid userId, Guid groupId)
    {
        var files = await _dataContext.GroupFiles
            .Where(gf => gf.GroupId == groupId)
            .Select(gf => gf.File)
            .ToListAsync();
        var frontendFiles = files.Where(f => _filePolicyChecker.FileDownloadable(f, userId))
                                 .Select(f => _learnzFrontendFileGenerator.FrontendFileHistorized(f, userId))
                                 .ToList();
        return frontendFiles;
    }

    public async Task<GroupMessageChatDTO> GetMessages(Guid userId, Guid groupId)
    {
        var messages = await _dataContext.GroupMessages.Where(msg => msg.GroupId == groupId)
            .OrderByDescending(msg => msg.Date)
            .Select(msg => new GroupMessageGetDTO
            {
                Message = msg.Message,
                UserName = msg.Sender.Username,
                Date = msg.Date,
                SentByMe = msg.SenderId == userId,
                IsInfoMessage = msg.IsInfoMessage
            })
            .ToListAsync();
        var group = await _dataContext.Groups.Include(g => g.ProfileImage).FirstAsync(g => g.Id == groupId);
        var chat = new GroupMessageChatDTO
        {
            GroupName = group.Name,
            ProfileImagePath = _learnzFrontendFileGenerator.PathToImage(group.ProfileImage.ActualVersionPath),
            Messages = messages
        };
        return chat;
    }

    public async Task<List<GroupOverviewDTO>> GetGroupOverview(Guid userId)
    {
        var overview = await _dataContext.Groups.Where(grp => grp.GroupMembers.Select(gm => gm.UserId).Contains(userId))
            .Select(grp => new GroupOverviewDTO
            {
                GroupId = grp.Id,
                GroupName = grp.Name,
                ProfileImagePath = _learnzFrontendFileGenerator.PathToImage(grp.ProfileImage.ActualVersionPath),
                LastMessage = grp.GroupMessages.Any()
                    ? grp.GroupMessages.OrderByDescending(grp => grp.Date).First().Message
                    : null,
                LastMessageDateSent = grp.GroupMessages.Any()
                    ? grp.GroupMessages.OrderByDescending(grp => grp.Date).First().Date
                    : null,
                LastMessageSentUsername = grp.GroupMessages.Any()
                    ? grp.GroupMessages.OrderByDescending(grp => grp.Date).First().Sender.Username
                    : null,
                LastMessageSentByMe = grp.GroupMessages.Any()
                    ? grp.GroupMessages.OrderByDescending(grp => grp.Date).First().SenderId == userId
                    : null,
                LastMessageWasInfoMessage = grp.GroupMessages.Any()
                    ? grp.GroupMessages.OrderByDescending(grp => grp.Date).First().IsInfoMessage
                    : null,
                NumberOfFiles = grp.GroupFiles.Count()
            })
            .OrderByDescending(grp => grp.LastMessageDateSent)
            .ToListAsync();
        return overview;
    }
}