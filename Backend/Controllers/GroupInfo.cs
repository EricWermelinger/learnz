using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace Learnz.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class GroupInfo : Controller
{
    private readonly DataContext _dataContext;
    private readonly IUserService _userService;
    private readonly IFileFinder _fileFinder;
    private readonly IFilePolicyChecker _filePolicyChecker;
    private readonly IPathToImageConverter _pathToImageConverter;
    private readonly IGroupQueryService _groupQueryService;
    private readonly HubService _hubService;
    public GroupInfo(DataContext dataContext, IUserService userService, IFileFinder fileFinder, IFilePolicyChecker filePolicyChecker, IPathToImageConverter pathToImageConverter, IGroupQueryService groupQueryService, IHubContext<LearnzHub> learnzHub)
    {
        _dataContext = dataContext;
        _userService = userService;
        _fileFinder = fileFinder;
        _filePolicyChecker = filePolicyChecker;
        _pathToImageConverter = pathToImageConverter;
        _groupQueryService = groupQueryService;
        _hubService = new HubService(learnzHub);
    }

    [HttpGet]
    public async Task<ActionResult<GroupInfoDTO>> GetGroupInfo(Guid groupId)
    {
        var guid = _userService.GetUserGuid();
        if (!(await _dataContext.GroupMembers.AnyAsync(gm => gm.GroupId == groupId && gm.UserId == guid)))
        {
            return BadRequest(ErrorKeys.AccessBlocked);
        }

        var group = await _dataContext.Groups
            .Select(g => new GroupInfoDTO
            {
                GroupId = g.Id,
                Name = g.Name,
                Description = g.Description,
                ProfileImagePath = _pathToImageConverter.PathToImage(g.ProfileImage.Path),
                ProfileImageName = g.ProfileImage.FileNameExternal,
                Members = g.GroupMembers.Select(gm => new GroupInfoMemberDTO
                                                    {
                                                        UserId = gm.UserId,
                                                        Firstname = gm.User.Firstname,
                                                        Lastname = gm.User.Lastname,
                                                        Username = gm.User.Username,
                                                        IsAdmin = g.AdminId == gm.UserId,
                                                        ProfileImagePath = _pathToImageConverter.PathToImage(gm.User.ProfileImage.Path)
                                                    })
                                                    .OrderByDescending(gm => gm.IsAdmin)
                                                    .ThenBy(gm => gm.Username)
                                                    .ToList(),
                IsUserAdmin = g.AdminId == guid
            })
            .FirstAsync();
        return Ok(group);
    }

    [HttpPost]
    public async Task<ActionResult> UpsertGroup(GroupInfoCreateDTO request)
    {
        var guid = _userService.GetUserGuid();
        var existingGroup = await _dataContext.Groups.FirstOrDefaultAsync(g => g.Id == request.GroupId);
        var timestamp = DateTime.UtcNow;

        var profileImageId = await _fileFinder.GetFileId(_dataContext, guid, request.ProfileImagePath, _filePolicyChecker);
        if (profileImageId == null)
        {
            return BadRequest(ErrorKeys.FileNotValid);
        }

        if (existingGroup == null)
        {
            await _dataContext.Groups.AddAsync(new Group
            {
                Id = request.GroupId,
                AdminId = guid,
                Description = request.Description,
                Name = request.Name,
                ProfileImageId = (Guid)profileImageId
            });
            await _dataContext.SaveChangesAsync();
            var members = await _dataContext.Users.Where(u => request.Members.Contains(u.Id)).ToListAsync();
            foreach (User member in members)
            {
                await _dataContext.GroupMembers.AddAsync(new GroupMember
                {
                    GroupId = request.GroupId,
                    UserId = member.Id
                });
            }
            await _dataContext.GroupMembers.AddAsync(new GroupMember
            {
                GroupId = request.GroupId,
                UserId = guid
            });

            var createdMessage = new GroupMessage
            {
                Date = timestamp,
                GroupId = request.GroupId,
                IsInfoMessage = true,
                Message = "groupCreated|" + request.Name,
                SenderId = guid
            };
            await _dataContext.GroupMessages.AddAsync(createdMessage);
        }
        else
        {
            existingGroup.Description = request.Description;
            existingGroup.Name = request.Name;
            existingGroup.ProfileImageId = (Guid)profileImageId;
            var existingMember =
                await _dataContext.GroupMembers.Where(gm => gm.GroupId == request.GroupId).ToListAsync();
            var addedMembers = await _dataContext.Users.Where(u =>
                request.Members.Contains(u.Id) && !existingMember.Select(em => em.Id).Contains(u.Id))
                .ToListAsync();
            var deletedMembers = await _dataContext.GroupMembers.Where(gm => gm.GroupId == request.GroupId && !request.Members.Contains(gm.UserId))
                .ToListAsync();
            var newUsers = addedMembers.Select(am => new GroupMember
            {
                GroupId = request.GroupId,
                UserId = am.Id
            });
            var deletedMembersMessage = deletedMembers.Select(dm => new GroupMessage
            {
                GroupId = request.GroupId,
                IsInfoMessage = true,
                Date = timestamp,
                Message = "userDeleted|" + dm.User.Username,
                SenderId = guid
            });
            var addedMembersMessage = addedMembers.Select(am => new GroupMessage
            {
                GroupId = request.GroupId,
                IsInfoMessage = true,
                Date = timestamp,
                Message = "userAdded|" + am.Username,
                SenderId = guid
            });

            _dataContext.GroupMembers.RemoveRange(deletedMembers);
            await _dataContext.GroupMembers.AddRangeAsync(newUsers);
            await _dataContext.GroupMessages.AddRangeAsync(deletedMembersMessage);
            await _dataContext.GroupMessages.AddRangeAsync(addedMembersMessage);
        }

        await _dataContext.SaveChangesAsync();

        var groupMembersIds = await _dataContext.GroupMembers.Where(gm => gm.GroupId == request.GroupId)
                                                             .Select(gm => gm.UserId)
                                                             .ToListAsync();
        foreach (var memberId in groupMembersIds)
        {
            var groupOverview = await _groupQueryService.GetGroupOverview(memberId);
            var chatMessages = await _groupQueryService.GetMessages(memberId, request.GroupId);
            await _hubService.SendMessageToUser(nameof(GroupOverview), groupOverview, memberId);
            await _hubService.SendMessageToUser(nameof(GroupOverview), chatMessages, memberId, request.GroupId);
        }

        return Ok();
    }
}
