using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Learnz.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class DrawCollections : Controller
{
    private readonly DataContext _dataContext;
    private readonly IUserService _userService;
    private readonly IDrawQueryService _drawQueryService;
    public DrawCollections(DataContext dataContext, IUserService userService, IDrawQueryService drawQueryService)
    {
        _dataContext = dataContext;
        _userService = userService;
        _drawQueryService = drawQueryService;
    }

    [HttpGet]
    public async Task<ActionResult<List<DrawCollectionGetDTO>>> GetCollections()
    {
        var guid = _userService.GetUserGuid();
        var result = await _drawQueryService.GetCollections(guid);
        return Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult> UpsertCollection(DrawCollectionUpsertDTO request)
    {
        var guid = _userService.GetUserGuid();
        bool isUserInGroup = request.GroupId == null || await _dataContext.GroupMembers.AnyAsync(grm => grm.UserId == guid);
        if (!isUserInGroup)
        {
            return BadRequest(ErrorKeys.GroupNotAccessible);
        }
        var timeStamp = DateTime.UtcNow;
        var existingCollection = await _dataContext.DrawCollections.FirstOrDefaultAsync(drc => drc.Id == request.CollectionId);
        if (existingCollection != null)
        {
            if (existingCollection.OwnerId != guid)
            {
                return BadRequest(ErrorKeys.DrawNotAccessible);
            }
            existingCollection.Name = request.Name;
            if (request.GroupId != null && request.DrawGroupPolicy != null)
            {
                var existingGroupCollection = await _dataContext.DrawGroupCollections.FirstOrDefaultAsync(dgr => dgr.DrawCollectionId == request.CollectionId);
                if (existingGroupCollection == null || (existingGroupCollection != null && existingGroupCollection.GroupId != request.GroupId))
                {
                    var newGroupCollection = new DrawGroupCollection
                    {
                        GroupId = (Guid)request.GroupId,
                        DrawGroupPolicy = (DrawGroupPolicy)request.DrawGroupPolicy,
                        DrawCollectionId = request.CollectionId
                    };
                    _dataContext.DrawGroupCollections.Add(newGroupCollection);
                    if (existingGroupCollection != null && existingGroupCollection.GroupId != request.GroupId)
                    {
                        _dataContext.DrawGroupCollections.Remove(existingGroupCollection);
                    }
                }
                else
                {
                    existingGroupCollection!.DrawGroupPolicy = (DrawGroupPolicy)request.DrawGroupPolicy;
                }
            }
            else if (request.GroupId == null)
            {
                var existingGroupCollection = await _dataContext.DrawGroupCollections.FirstOrDefaultAsync(dgr => dgr.DrawCollectionId == request.CollectionId);
                if (existingGroupCollection != null && existingGroupCollection.GroupId != null)
                {
                    _dataContext.DrawGroupCollections.Remove(existingGroupCollection);
                }
            }
        }
        else if (request.FirstPageId != null)
        {
            var newDrawCollection = new DrawCollection
            {
                Id = request.CollectionId,
                Changed = timeStamp,
                ChangedById = guid,
                Name = request.Name,
                OwnerId = guid
            };
            _dataContext.DrawCollections.Add(newDrawCollection);
            await _dataContext.SaveChangesAsync();

            var newDrawCollectionFirstPage = new DrawPage
            {
                Id = (Guid)request.FirstPageId,
                Changed = timeStamp,
                ChangedById = guid,
                Created = timeStamp,
                DataUrl = "",
                OwnerId = guid,
                DrawCollectionId = newDrawCollection.Id,
            };
            _dataContext.DrawPages.Add(newDrawCollectionFirstPage);
            await _dataContext.SaveChangesAsync();

            if (request.GroupId != null && request.DrawGroupPolicy != null)
            {
                var newGroupCollection = new DrawGroupCollection
                {
                    GroupId = (Guid)request.GroupId,
                    DrawGroupPolicy = (DrawGroupPolicy)request.DrawGroupPolicy,
                    DrawCollectionId = request.CollectionId
                };
                _dataContext.DrawGroupCollections.Add(newGroupCollection);
            }
        }
        await _dataContext.SaveChangesAsync();
        await _drawQueryService.TriggerWebsocketCollections(request.CollectionId, request.GroupId, request.GroupId == null ? guid : null);
        return Ok();
    }

    [HttpDelete]
    public async Task<ActionResult> DeleteCollection(Guid collectionId)
    {
        var guid = _userService.GetUserGuid();
        var collection = await _dataContext.DrawCollections.FirstOrDefaultAsync(dcl => dcl.Id == collectionId);
        if (collection == null || collection.OwnerId != guid)
        {
            return BadRequest(ErrorKeys.DrawNotAccessible);
        }

        var groupCollection = await _dataContext.DrawGroupCollections.FirstOrDefaultAsync(dgr => dgr.DrawCollectionId == collectionId);
        var isGroupMember = groupCollection == null ? true : await _dataContext.GroupMembers.AnyAsync(dgr => dgr.GroupId == groupCollection.GroupId && dgr.UserId == guid);
        if (!isGroupMember)
        {
            return BadRequest(ErrorKeys.DrawNotAccessible);
        }

        Guid? groupId = groupCollection == null ? null : groupCollection.GroupId;
        Guid? userId = groupCollection == null ? guid : null;

        var pages = await _dataContext.DrawPages.Where(dpg => dpg.DrawCollectionId == collectionId).ToListAsync();
        foreach (var page in pages)
        {
            var existingStorages = await _dataContext.DrawCanvasStorages.Where(dcs => dcs.DrawPageId == page.Id).ToListAsync();
            var existingStoragePoints = await _dataContext.DrawCanvasStoragePoints.Where(dcp => dcp.DrawCanvasStoragesFrom.Any(dcs => dcs.DrawPageId == page.Id)).ToListAsync();
            _dataContext.DrawCanvasStorages.RemoveRange(existingStorages);
            _dataContext.DrawCanvasStoragePoints.RemoveRange(existingStoragePoints);
        }

        _dataContext.DrawPages.RemoveRange(pages);
        await _dataContext.SaveChangesAsync();

        if (groupCollection != null)
        {
            _dataContext.DrawGroupCollections.Remove(groupCollection);
            await _dataContext.SaveChangesAsync();
        }

        _dataContext.DrawCollections.Remove(collection);
        await _dataContext.SaveChangesAsync();

        await _drawQueryService.TriggerWebsocketCollections(collectionId, groupId, userId);
        return Ok();
    }
}