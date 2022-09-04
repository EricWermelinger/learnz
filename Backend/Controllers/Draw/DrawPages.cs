using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Learnz.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class DrawPages : Controller
{
    private readonly DataContext _dataContext;
    private readonly IUserService _userService;
    private readonly IDrawQueryService _drawQueryService;
    private readonly IDrawPolicyChecker _drawPolicyChecker;
    public DrawPages(DataContext dataContext, IUserService userService, IDrawQueryService drawQueryService, IDrawPolicyChecker drawPolicyChecker)
    {
        _dataContext = dataContext;
        _userService = userService;
        _drawQueryService = drawQueryService;
        _drawPolicyChecker = drawPolicyChecker;
    }

    [HttpGet]
    public async Task<ActionResult<DrawDrawingDTO>> GetPages(Guid collectionId)
    {
        var guid = _userService.GetUserGuid();
        var result = await _drawQueryService.GetPages(guid, collectionId);
        if (result == null)
        {
            return BadRequest(ErrorKeys.DrawNotAccessible);
        }
        return Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult> CreatePage(DrawPageCreateDTO request)
    {
        var guid = _userService.GetUserGuid();
        var timeStamp = DateTime.UtcNow;
        var existing = await _dataContext.DrawPages.AnyAsync(dpg => dpg.Id == request.PageId);
        if (!existing)
        {
            var newPage = new DrawPage
            {
                Id = request.PageId,
                Changed = timeStamp,
                ChangedById = guid,
                Created = timeStamp,
                DataUrl = "",
                DrawCollectionId = request.CollectionId,
                OwnerId = guid
            };
            _dataContext.DrawPages.Add(newPage);
            await _dataContext.SaveChangesAsync();
            await _drawQueryService.AdjustChangedOnCollection(request.CollectionId, guid);
            var groupId = await _dataContext.DrawGroupCollections.Where(dgc => dgc.DrawCollectionId == request.CollectionId).Select(dgc => dgc.GroupId).FirstOrDefaultAsync();
            await _drawQueryService.TriggerWebsocketCollections(request.CollectionId, groupId, groupId == Guid.Empty ? guid : null);
            await _drawQueryService.TriggerWebsocketPages(request.CollectionId);
        }
        return Ok();
    }

    [HttpPut]
    public async Task<ActionResult> EditPage(DrawPageEditDTO request)
    {
        var guid = _userService.GetUserGuid();
        var timestamp = DateTime.UtcNow;
        var existingPage = await _dataContext.DrawPages.FirstOrDefaultAsync(dpg => dpg.Id == request.PageId && dpg.DrawCollectionId == request.CollectionId);
        if (existingPage == null)
        {
            return BadRequest(ErrorKeys.DrawNotAccessible);
        }
        var groupPage = await _dataContext.DrawGroupCollections.Where(dgr => dgr.DrawCollection.DrawPages.Any(dpg => dpg.Id == request.PageId))
                                                                 .Select(dgr => new
                                                                 {
                                                                     Policy = dgr.DrawGroupPolicy,
                                                                     OwnerId = dgr.DrawCollection.OwnerId
                                                                 })
                                                                 .FirstOrDefaultAsync();
        if (groupPage != null && !_drawPolicyChecker.GroupPageEditable(groupPage.Policy, groupPage.OwnerId, guid))
        {
            return BadRequest(ErrorKeys.DrawNotAccessible);
        }

        var isPrivateCollectionOwner = groupPage != null || await _dataContext.DrawPages.AnyAsync(dpg => dpg.Id == request.PageId && dpg.DrawCollectionId == request.CollectionId && dpg.OwnerId == guid);
        if (!isPrivateCollectionOwner)
        {
            return BadRequest(ErrorKeys.DrawNotAccessible);
        }

        bool currentlyOtherUserEditing = existingPage.Changed.AddMinutes(1) > timestamp && existingPage.ChangedById != guid;
        if (currentlyOtherUserEditing)
        {
            await _drawQueryService.TriggerWebsocketNewUserEditing(request.CollectionId, existingPage.ChangedById, guid);
        }

        existingPage.Changed = timestamp;
        existingPage.ChangedById = guid;
        existingPage.DataUrl = request.DataUrl;
        await _dataContext.SaveChangesAsync();
        await _drawQueryService.AdjustChangedOnCollection(request.CollectionId, guid);
        var groupId = await _dataContext.DrawGroupCollections.Where(dgc => dgc.DrawCollectionId == request.CollectionId).Select(dgc => dgc.GroupId).FirstOrDefaultAsync();
        await _drawQueryService.TriggerWebsocketCollections(request.CollectionId, groupId, groupId == Guid.Empty ? guid : null);
        await _drawQueryService.TriggerWebsocketPages(request.CollectionId);
        return Ok();
    }

    [HttpDelete]
    public async Task<ActionResult> DeletePage(Guid collectionId, Guid pageId)
    {
        var guid = _userService.GetUserGuid();
        var existingPage = await _dataContext.DrawPages.FirstOrDefaultAsync(dpg => dpg.OwnerId == guid && dpg.Id == pageId && dpg.DrawCollectionId == collectionId && dpg.DrawCollection.DrawPages.Count > 1);
        if (existingPage == null)
        {
            return BadRequest(ErrorKeys.DrawNotAccessible);
        }
        _dataContext.DrawPages.Remove(existingPage);
        await _dataContext.SaveChangesAsync();
        await _drawQueryService.AdjustChangedOnCollection(collectionId, guid);
        var groupId = await _dataContext.DrawGroupCollections.Where(dgc => dgc.DrawCollectionId == collectionId).Select(dgc => dgc.GroupId).FirstOrDefaultAsync();
        await _drawQueryService.TriggerWebsocketCollections(collectionId, groupId, groupId == Guid.Empty ? guid : null);
        await _drawQueryService.TriggerWebsocketPages(collectionId);
        return Ok();
    }
}