using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace Learnz.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class GroupFiles : Controller
{
    private readonly DataContext _dataContext;
    private readonly IUserService _userService;
    private readonly IFilePolicyChecker _filePolicyChecker;
    private readonly IGroupQueryService _groupQueryService;
    private readonly HubService _hubService;
    public GroupFiles(DataContext dataContext, IUserService userService, IFilePolicyChecker filePolicyChecker, IGroupQueryService groupQueryService, IHubContext<LearnzHub> learnzHub)
    {
        _dataContext = dataContext;
        _userService = userService;
        _filePolicyChecker = filePolicyChecker;
        _groupQueryService = groupQueryService;
        _hubService = new HubService(learnzHub);
    }

    [HttpGet]
    public async Task<ActionResult<List<FileInfoDTO>>> GetFiles(Guid groupId)
    {
        var guid = _userService.GetUserGuid();
        if (!(await _dataContext.GroupMembers.AnyAsync(gm => gm.GroupId == groupId && gm.UserId == guid)))
        {
            return BadRequest(ErrorKeys.FileNotAccessible);
        }

        var files = await _groupQueryService.GetFiles(guid, groupId);
        return Ok(files);
    }

    [HttpPost]
    public async Task<ActionResult> EditFiles(GroupFilesEditDTO request)
    {
        var user = await _userService.GetUser();
        var guid = user.Id;
        if (!(await _dataContext.GroupMembers.AnyAsync(gm => gm.GroupId == request.GroupId && gm.UserId == guid)))
        {
            return BadRequest(ErrorKeys.FileNotAccessible);
        }
        
        var addedFiles = await _dataContext.Files
            .Where(f => request.FilesAdded.Contains(f.Id))
            .ToListAsync();

        var deletedFiles = await _dataContext.Files
            .Where(f => request.FilesDeleted.Contains(f.Id))
            .ToListAsync();

        foreach (LearnzFile file in deletedFiles)
        {
            if (!_filePolicyChecker.FileDeletable(file, guid))
            {
                return BadRequest(ErrorKeys.FileNotAccessible);
            }
        }
        
        var existingGroupFiles = await _dataContext.GroupFiles
            .Where(gf => gf.GroupId == request.GroupId)
            .ToListAsync();

        foreach (LearnzFile file in deletedFiles)
        {
            _dataContext.GroupFiles.Remove(existingGroupFiles.First(gf => gf.FileId == file.Id));
            _dataContext.Files.Remove(file);
        }

        foreach (LearnzFile file in addedFiles)
        {
            await _dataContext.GroupFiles.AddAsync(new GroupFile
            {
                GroupId = request.GroupId,
                FileId = file.Id
            });
        }

        var filesEditedMessage = new GroupMessage
        {
            GroupId = request.GroupId,
            IsInfoMessage = true,
            Date = DateTime.UtcNow,
            Message = "filesEdited|" + user.Username,
            SenderId = guid
        };
        _dataContext.GroupMessages.Add(filesEditedMessage);

        await _dataContext.SaveChangesAsync();

        var groupMembersIds = await _dataContext.GroupMembers.Where(gm => gm.GroupId == request.GroupId)
                                                             .Select(gm => gm.UserId)
                                                             .ToListAsync();
        foreach (var memberId in groupMembersIds)
        {
            var fileOverview = await _groupQueryService.GetFiles(memberId, request.GroupId);
            var groupOverview = await _groupQueryService.GetGroupOverview(memberId);
            var chatMessages = await _groupQueryService.GetMessages(memberId, request.GroupId);
            await _hubService.SendMessageToUser(nameof(GroupFiles), fileOverview, memberId, request.GroupId);
            await _hubService.SendMessageToUser(nameof(GroupOverview), groupOverview, memberId);
            await _hubService.SendMessageToUser(nameof(GroupOverview), chatMessages, memberId, request.GroupId);
        }

        return Ok();
    }
}
