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
    public async Task<ActionResult<List<FileFrontendHistorizedDTO>>> GetFiles(Guid groupId)
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
    public async Task<ActionResult> AddFile(GroupFileChangeDTO request)
    {
        var user = await _userService.GetUser();
        var guid = user.Id;
        var file = await _dataContext.Files.FirstOrDefaultAsync(f => f.ActualVersionPath == request.Path);
        if (!(await _dataContext.GroupMembers.AnyAsync(gm => gm.GroupId == request.GroupId && gm.UserId == guid)) || file == null)
        {
            return BadRequest(ErrorKeys.FileNotAccessible);
        }

        var newFile = new GroupFile
        {
            FileId = file.Id,
            GroupId = request.GroupId,
        };

        bool visible = _filePolicyChecker.GroupFileVisible(file);
        if (visible)
        {
            var message = new GroupMessage
            {
                GroupId = request.GroupId,
                IsInfoMessage = true,
                Date = DateTime.UtcNow,
                Message = "fileAdded|" + user.Username + " - " + file.ActualVersionFileNameExternal,
                SenderId = guid
            };
            _dataContext.GroupMessages.Add(message);
            await _dataContext.SaveChangesAsync();
        }

        _dataContext.GroupFiles.Add(newFile);
        await _dataContext.SaveChangesAsync();

        await TriggerWebsockets(request.GroupId, visible, guid);

        return Ok();
    }

    [HttpPut]
    public async Task<ActionResult> UpdateFile(GroupFileChangeDTO request)
    {
        var user = await _userService.GetUser();
        var file = await _dataContext.Files.FirstOrDefaultAsync(f => f.ActualVersionPath == request.Path);
        if (file == null)
        {
            return BadRequest(ErrorKeys.FileNotAccessible);
        }
        bool visible = _filePolicyChecker.GroupFileVisible(file);
        if (visible)
        {
            var message = new GroupMessage
            {
                GroupId = request.GroupId,
                IsInfoMessage = true,
                Date = DateTime.UtcNow,
                Message = "fileEdited|" + user.Username + " - " + file.ActualVersionFileNameExternal,
                SenderId = user.Id
            };
            _dataContext.GroupMessages.Add(message);
            await _dataContext.SaveChangesAsync();
        }            
        await TriggerWebsockets(request.GroupId, true, user.Id);
        return Ok();
    }

    [HttpDelete]
    public async Task<ActionResult> DeleteFile(Guid groupId, string path)
    {
        var user = await _userService.GetUser();
        var guid = user.Id;

        var file = await _dataContext.Files.FirstOrDefaultAsync(f => f.ActualVersionPath == path);
        if (file == null || !_filePolicyChecker.FileDeletable(file, guid))
        {
            return BadRequest(ErrorKeys.FileNotAccessible);
        }
        bool visible = _filePolicyChecker.GroupFileVisible(file);
        if (visible)
        {
            var message = new GroupMessage
            {
                GroupId = groupId,
                IsInfoMessage = true,
                Date = DateTime.UtcNow,
                Message = "fileDeleted|" + user.Username + " - " + file.ActualVersionFileNameExternal,
                SenderId = user.Id
            };
            _dataContext.GroupMessages.Add(message);
        }
        try
        {
            System.IO.File.Delete(path);
        }
        catch { }
        var versions = await _dataContext.FileVersions.Where(lvf => lvf.FileId == file.Id).ToListAsync();
        var groupFile = await _dataContext.GroupFiles.FirstOrDefaultAsync(gf => gf.File.ActualVersionPath == path);
        _dataContext.RemoveRange(versions);
        await _dataContext.SaveChangesAsync();
        _dataContext.Remove(file);
        await _dataContext.SaveChangesAsync();
        await TriggerWebsockets(groupId, visible, guid);
        return Ok();
    }

    private async Task TriggerWebsockets(Guid groupId, bool visible, Guid userId)
    {
        if (visible)
        {
            var groupMembersIds = await _dataContext.GroupMembers.Where(gm => gm.GroupId == groupId)
                                                             .Select(gm => gm.UserId)
                                                             .ToListAsync();
            foreach (var memberId in groupMembersIds)
            {
                var fileOverview = await _groupQueryService.GetFiles(memberId, groupId);
                var groupOverview = await _groupQueryService.GetGroupOverview(memberId);
                var chatMessages = await _groupQueryService.GetMessages(memberId, groupId);
                await _hubService.SendMessageToUser(nameof(GroupFiles), fileOverview, memberId, groupId);
                await _hubService.SendMessageToUser(nameof(GroupOverview), groupOverview, memberId);
                await _hubService.SendMessageToUser(nameof(GroupMessages), chatMessages, memberId, groupId);
            }
        }
        else
        {
            var fileOverview = await _groupQueryService.GetFiles(userId, groupId);
            var groupOverview = await _groupQueryService.GetGroupOverview(userId);
            var chatMessages = await _groupQueryService.GetMessages(userId, groupId);
            await _hubService.SendMessageToUser(nameof(GroupFiles), fileOverview, userId, groupId);
            await _hubService.SendMessageToUser(nameof(GroupOverview), groupOverview, userId);
            await _hubService.SendMessageToUser(nameof(GroupMessages), chatMessages, userId, groupId);
        }
    }
}
