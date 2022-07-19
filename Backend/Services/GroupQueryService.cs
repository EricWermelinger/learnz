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

    public async Task<List<FileInfoDTO>> GetFiles(Guid userId, Guid groupId)
    {
        var files = await _dataContext.GroupFiles
            .Where(gf => gf.GroupId == groupId)
            .Select(gf => new FileInfoDTO
            {
                FileNameExternal = gf.File.FileNameExternal,
                FilePath = gf.File.Path,
                Created = gf.File.Created,
                CreatedUsername = gf.File.CreatedBy.Username,
                Modified = gf.File.Modified,
                ModifiedUsername = gf.File.ModifiedBy.Username,
                FileFromMe = gf.File.CreatedById == userId,
                FileEditable = _filePolicyChecker.FileEditable(gf.File, userId),
                FileDeletable = _filePolicyChecker.FileDeletable(gf.File, userId)
            })
            .ToListAsync();

        return files;
    }

    public async Task<GroupMessageChatDTO> GetMessages(Guid userId, Guid groupId)
    {
        var messages = await _dataContext.GroupMessages.Where(msg => msg.GroupId == groupId)
            .OrderByDescending(msg => msg.Date)
            .Select(msg => new GroupMessageGetDTO
            {
                Message = msg.Message,
                UserName = msg.Sender.Firstname + " " + msg.Sender.Lastname,
                Date = msg.Date,
                SentByMe = msg.SenderId == userId,
                IsInfoMessage = msg.IsInfoMessage
            })
            .ToListAsync();
        var group = await _dataContext.Groups.Include(g => g.ProfileImage).FirstAsync(g => g.Id == groupId);
        var chat = new GroupMessageChatDTO
        {
            GroupName = group.Name,
            ProfileImagePath = _learnzFrontendFileGenerator.PathToImage(group.ProfileImage.Path),
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
                ProfileImagePath = _learnzFrontendFileGenerator.PathToImage(grp.ProfileImage.Path),
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