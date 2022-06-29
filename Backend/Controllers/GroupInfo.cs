using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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

    public GroupInfo(DataContext dataContext, IUserService userService, IFileFinder fileFinder, IFilePolicyChecker filePolicyChecker)
    {
        _dataContext = dataContext;
        _userService = userService;
        _fileFinder = fileFinder;
        _filePolicyChecker = filePolicyChecker;
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
                ProfileImagePath = g.ProfileImage.Path,
                Members = g.GroupMembers.Select(gm => new GroupInfoMemberDTO
                {
                    UserId = gm.UserId,
                    Firstname = gm.User.Firstname,
                    Lastname = gm.User.Lastname,
                    Username = gm.User.Username,
                    IsAdmin = g.AdminId == gm.UserId,
                    ProfileImagePath = gm.User.ProfileImage.Path
                }).ToList()
            })
            .FirstAsync();
        return Ok(group);
    }

    [HttpPost]
    public async Task<ActionResult> CreateGroup(GroupInfoCreateDTO request)
    {
        var guid = _userService.GetUserGuid();
        var existingGroup = await _dataContext.Groups.FirstOrDefaultAsync(g => g.Id == request.GroupId);
        var timestamp = DateTime.UtcNow;

        var profileImageId =
            await _fileFinder.GetFileId(_dataContext, guid, request.ProfileImagePath, _filePolicyChecker);
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

            var createdMessage = new GroupMessage
            {
                Date = timestamp,
                GroupId = request.GroupId,
                IsInfoMessage = true,
                Message = "groupCreated",
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
        return Ok();
    }
}
