using Learnz.Controllers;
using Microsoft.AspNetCore.SignalR;

namespace Learnz.Services;
public class DrawQueryService : IDrawQueryService
{
    private readonly DataContext _dataContext;
    private readonly HubService _hubService;
    private readonly IDrawPolicyChecker _drawPolicyChecker;
    public DrawQueryService(DataContext dataContext, IHubContext<LearnzHub> learnzHub, IDrawPolicyChecker drawPolicyChecker)
    {
        _dataContext = dataContext;
        _hubService = new HubService(learnzHub);
        _drawPolicyChecker = drawPolicyChecker;
    }
    
    public async Task<List<DrawCollectionGetDTO>> GetCollections(Guid userId)
    {
        var ownCollections = await _dataContext.DrawCollections.Where(drc => drc.OwnerId == userId)
                                                               .Select(drc => new DrawCollectionGetDTO
                                                               {
                                                                   CollectionId = drc.Id,
                                                                   Deletable = true,
                                                                   Editable = true,
                                                                   FirstPageId = drc.DrawPages.OrderBy(p => p.Created).First().Id,
                                                                   GroupName = drc.DrawGroupCollections.Any() ? drc.DrawGroupCollections.First().Group.Name : null,
                                                                   GroupId = drc.DrawGroupCollections.Any() ? drc.DrawGroupCollections.First().GroupId : null,
                                                                   DrawGroupPolicy = drc.DrawGroupCollections.Any() ? drc.DrawGroupCollections.First().DrawGroupPolicy : null,
                                                                   IsGroupCollection = drc.DrawGroupCollections.Any(),
                                                                   LastChanged = drc.Changed,
                                                                   LastChangedBy = drc.ChangedBy.Username,
                                                                   Name = drc.Name,
                                                                   NumberOfPages = drc.DrawPages.Count()
                                                               })
                                                               .ToListAsync();

        var groupCollections = await _dataContext.DrawGroupCollections.Where(dgr => dgr.Group.GroupMembers.Any(grm => grm.UserId == userId))
                                                                      .Where(dgr => !ownCollections.Select(c => c.CollectionId).Any(id => dgr.DrawCollectionId == id))
                                                                      .Select(drc => drc.DrawCollection)
                                                                      .Select(drc => new
                                                                      {
                                                                          Collection = new DrawCollectionGetDTO
                                                                          {
                                                                              CollectionId = drc.Id,
                                                                              Deletable = false,
                                                                              Editable = false,
                                                                              FirstPageId = drc.DrawPages.OrderBy(p => p.Created).First().Id,
                                                                              GroupName = drc.DrawGroupCollections.Any() ? drc.DrawGroupCollections.First().Group.Name : null,
                                                                              GroupId = drc.DrawGroupCollections.Any() ? drc.DrawGroupCollections.First().GroupId : null,
                                                                              DrawGroupPolicy = drc.DrawGroupCollections.Any() ? drc.DrawGroupCollections.First().DrawGroupPolicy : null,
                                                                              IsGroupCollection = drc.DrawGroupCollections.Any(),
                                                                              LastChanged = drc.Changed,
                                                                              LastChangedBy = drc.ChangedBy.Username,
                                                                              Name = drc.Name,
                                                                              NumberOfPages = drc.DrawPages.Count(),
                                                                          },
                                                                          Policy = drc.DrawGroupCollections.First().DrawGroupPolicy,
                                                                          OwnerId = drc.OwnerId
                                                                      })
                                                                      .ToListAsync();
        
        var groupCollectionsWithPolicy = groupCollections.Select(drc => new DrawCollectionGetDTO
                                                                    {
                                                                        CollectionId = drc.Collection.CollectionId,
                                                                        Deletable = _drawPolicyChecker.GroupPageDeletable(drc.Policy, drc.OwnerId, userId),
                                                                        Editable = _drawPolicyChecker.GroupPageEditable(drc.Policy, drc.OwnerId, userId),
                                                                        FirstPageId = drc.Collection.FirstPageId,
                                                                        GroupName = drc.Collection.GroupName,
                                                                        IsGroupCollection = drc.Collection.IsGroupCollection,
                                                                        LastChanged = drc.Collection.LastChanged,
                                                                        LastChangedBy = drc.Collection.LastChangedBy,
                                                                        Name = drc.Collection.Name,
                                                                        NumberOfPages = drc.Collection.NumberOfPages
        }).ToList();
        if (ownCollections == null)
        {
            return (groupCollectionsWithPolicy ?? new List<DrawCollectionGetDTO>()).OrderByDescending(clc => clc.LastChangedBy).ToList();
        }
        ownCollections.AddRange(groupCollectionsWithPolicy ?? new List<DrawCollectionGetDTO>());
        return ownCollections.OrderByDescending(clc => clc.LastChangedBy).ToList();
    }

    public async Task<List<DrawPageGetDTO>?> GetPages(Guid userId, Guid collectionId)
    {
        bool? isOwn = await PagesIsOwn(userId, collectionId);
        if (isOwn == null)
        {
            return null;
        }
        var pages = await PagesWithoutPolicy(collectionId);
        var result = await PagesApplyPolicy(isOwn, pages, userId);
        return result;
    }

    private async Task<bool?> PagesIsOwn(Guid userId, Guid collectionId)
    {
        var isOwn = await _dataContext.DrawCollections.AnyAsync(drc => drc.OwnerId == userId && drc.Id == collectionId && !drc.DrawGroupCollections.Any());
        if (isOwn)
        {
            return true;
        }
        var isGroup = await _dataContext.DrawGroupCollections.AnyAsync(drg => drg.DrawCollectionId == collectionId && drg.Group.GroupMembers.Any(grm => grm.UserId == userId));
        return isGroup ? true : null;
    }

    private async Task<List<DrawPageGetBackendDTO>?> PagesWithoutPolicy(Guid collectionId)
    {
        var timestamp = DateTime.UtcNow;
        var pages = await _dataContext.DrawPages.Where(drp => drp.DrawCollectionId == collectionId)
                                                .OrderByDescending(drp => drp.Created)
                                                .Select(drp => new DrawPageGetBackendDTO
                                                {
                                                    DataUrl = drp.DataUrl,
                                                    Editable = false,
                                                    Deletable = false,
                                                    EditingPersonName = drp.Changed.AddMinutes(1) > timestamp ? drp.ChangedBy.Username : null,
                                                    EditingPersonProfileImagePath = drp.Changed.AddMinutes(1) > timestamp ? drp.ChangedBy.ProfileImage.Path : null,
                                                    PageId = drp.Id,
                                                    OwnerId = drp.OwnerId,
                                                    Policy = drp.DrawCollection.DrawGroupCollections.Any() ? drp.DrawCollection.DrawGroupCollections.First().DrawGroupPolicy : DrawGroupPolicy.Public,
                                                    PageCount = drp.DrawCollection.DrawPages.Count
                                                })
                                                .ToListAsync();
        return pages;
    }

    private async Task<List<DrawPageGetDTO>?> PagesApplyPolicy(bool? isOwn, List<DrawPageGetBackendDTO>? pages, Guid userId)
    {
        if (isOwn == null)
        {
            return null;
        }
        var pagesWithPolicy = pages.Select(dpg => new DrawPageGetDTO
        {
            DataUrl = dpg.DataUrl,
            Deletable = dpg.PageCount > 1 ? ((isOwn ?? true) ? true : _drawPolicyChecker.GroupPageDeletable(dpg.Policy, dpg.OwnerId, userId)) : false,
            Editable = dpg.PageCount > 1 ? ((isOwn ?? true) ? true : _drawPolicyChecker.GroupPageEditable(dpg.Policy, dpg.OwnerId, userId)) : false,
            EditingPersonName = dpg.EditingPersonName,
            EditingPersonProfileImagePath = dpg.EditingPersonProfileImagePath,
            PageId = dpg.PageId
        }).ToList();
        return pagesWithPolicy ?? new List<DrawPageGetDTO>();
    }

    public async Task TriggerWebsocketCollections(Guid collectionChangedId, Guid? groupId, Guid? userId)
    {
        List<Guid>? usersToNotify = new List<Guid>();
        if (groupId != null)
        {
            usersToNotify = await _dataContext.GroupMembers.Where(grm => grm.GroupId == groupId)
                                                           .Select(grm => grm.UserId)
                                                           .ToListAsync();
        }
        else if (userId != null)
        {
            usersToNotify.Add(userId ?? Guid.Empty);
        }
        else
        {
            usersToNotify = await _dataContext.DrawCollections.Where(drc => drc.Id == collectionChangedId)
                                                              .Select(drc => drc.DrawGroupCollections.Any() ? drc.DrawGroupCollections.First().Group.GroupMembers.Select(grm => grm.Id).ToList() : new List<Guid> { drc.OwnerId })
                                                              .SelectMany(usr => usr)
                                                              .ToListAsync();
        }
        foreach (Guid userToNotify in usersToNotify)
        {
            var data = await GetCollections(userToNotify);
            await _hubService.SendMessageToUser(nameof(DrawCollections), data, userToNotify);
        }
    }

    public async Task TriggerWebsocketPages(Guid collectionChangedId)
    {
        var usersToNotifyList = await _dataContext.DrawCollections.Where(drc => drc.Id == collectionChangedId)
                                                              .Select(drc => new
                                                              {
                                                                  UserIds = drc.DrawGroupCollections.Any() ? drc.DrawGroupCollections.First().Group.GroupMembers.Select(grm => grm.Id).ToList() : new List<Guid> { drc.OwnerId }
                                                              })
                                                              .ToListAsync();
        // todo here
        var usersToNotify = new List<Guid>();
        foreach (var userIds in usersToNotifyList)
        {
            if (userIds != null)
            {
                foreach (var userId in userIds)
                {
                    usersToNotify.Add(userId);
                }
            }
        }
        var pages = await PagesWithoutPolicy(collectionChangedId);
        foreach (Guid userToNotify in usersToNotify)
        {
            var isOwn = await PagesIsOwn(userToNotify, collectionChangedId);
            if (isOwn != null)
            {
                var data = PagesApplyPolicy(isOwn, pages, userToNotify);
                await _hubService.SendMessageToUser(nameof(DrawPages), data, userToNotify, collectionChangedId);
            }
        }
    }
}