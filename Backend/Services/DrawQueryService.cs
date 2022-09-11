using Learnz.Controllers;
using Microsoft.AspNetCore.SignalR;

namespace Learnz.Services;
public class DrawQueryService : IDrawQueryService
{
    private readonly DataContext _dataContext;
    private readonly HubService _hubService;
    private readonly IDrawPolicyChecker _drawPolicyChecker;
    private readonly ILearnzFrontendFileGenerator _learnzFrontendFileGenerator;

    public DrawQueryService(DataContext dataContext, IHubContext<LearnzHub> learnzHub, IDrawPolicyChecker drawPolicyChecker, ILearnzFrontendFileGenerator learnzFrontendFileGenerator)
    {
        _dataContext = dataContext;
        _hubService = new HubService(learnzHub);
        _drawPolicyChecker = drawPolicyChecker;
        _learnzFrontendFileGenerator = learnzFrontendFileGenerator;
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

    public async Task<DrawDrawingDTO?> GetPages(Guid userId, Guid collectionId)
    {
        bool? isOwn = await PagesIsOwn(userId, collectionId);
        if (isOwn == null)
        {
            return null;
        }
        var pages = await PagesWithoutPolicy(collectionId);
        var result = await PagesApplyPolicy(isOwn, pages, userId);
        var name = await PageCollectionName(collectionId);
        return new DrawDrawingDTO
        {
            Name = name ?? "",
            Pages = result ?? new List<DrawPageGetDTO>(),
            Editable = result != null && result.ElementAt(0).Editable,
            NewUserMakingChangesName = null
        };
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
        var currentEditing = await _dataContext.DrawPages.Where(drp => drp.DrawCollectionId == collectionId)
                                                         .Where(drp => drp.Changed.AddMinutes(1) > timestamp)
                                                         .OrderByDescending(drp => drp.Changed)
                                                         .Select(drp => drp.Id)
                                                         .FirstOrDefaultAsync();

        var pages = await _dataContext.DrawPages.Where(drp => drp.DrawCollectionId == collectionId)
                                                .OrderBy(drp => drp.Created)
                                                .Select(drp => new DrawPageGetBackendDTO
                                                {
                                                    DataUrl = drp.DataUrl,
                                                    Editable = false,
                                                    Deletable = false,
                                                    EditingPersonId = currentEditing == drp.Id ? drp.ChangedById : null,
                                                    EditingPersonName = currentEditing == drp.Id ? drp.ChangedBy.Username : null,
                                                    EditingPersonProfileImagePath = currentEditing == drp.Id ? drp.ChangedBy.ProfileImage.Path : null,
                                                    PageId = drp.Id,
                                                    OwnerId = drp.OwnerId,
                                                    Policy = drp.DrawCollection.DrawGroupCollections.Any() ? drp.DrawCollection.DrawGroupCollections.First().DrawGroupPolicy : DrawGroupPolicy.Public,
                                                    PageCount = drp.DrawCollection.DrawPages.Count,
                                                    IsEmpty = !drp.DrawCanvasStorages.Any(),
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
            Editable = ((isOwn ?? true) ? true : _drawPolicyChecker.GroupPageEditable(dpg.Policy, dpg.OwnerId, userId)),
            EditingPersonName = dpg.EditingPersonId == userId ? null : dpg.EditingPersonName,
            EditingPersonProfileImagePath = dpg.EditingPersonId == userId ? null : _learnzFrontendFileGenerator.PathToImage(dpg.EditingPersonProfileImagePath),
            PageId = dpg.PageId,
            IsEmpty = dpg.IsEmpty
        }).ToList();
        return pagesWithPolicy ?? new List<DrawPageGetDTO>();
    }

    private async Task<string?> PageCollectionName(Guid collectionId)
    {
        var name = await _dataContext.DrawCollections.Where(clc => clc.Id == collectionId).Select(clc => clc.Name).FirstOrDefaultAsync();
        return name;
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
                                                                .Select(drc => drc.DrawGroupCollections.Any() ? drc.DrawGroupCollections.First().Group.GroupMembers.Select(grm => grm.UserId).ToList() : new List<Guid> { drc.OwnerId })
                                                                .FirstOrDefaultAsync();
        }
        if (usersToNotify != null)
        {
            foreach (Guid userToNotify in usersToNotify)
            {
                var data = await GetCollections(userToNotify);
                await _hubService.SendMessageToUser(nameof(DrawCollections), data, userToNotify);
            }
        }
    }

    public async Task TriggerWebsocketPages(Guid collectionChangedId)
    {
        var usersToNotify = await _dataContext.DrawCollections.Where(drc => drc.Id == collectionChangedId)
                                                                .Select(drc => drc.DrawGroupCollections.Any() ? drc.DrawGroupCollections.First().Group.GroupMembers.Select(grm => grm.UserId).ToList() : new List<Guid> { drc.OwnerId })
                                                                .FirstOrDefaultAsync();
        var pages = await PagesWithoutPolicy(collectionChangedId);
        if (usersToNotify != null)
        {
            foreach (Guid userToNotify in usersToNotify)
            {
                var isOwn = await PagesIsOwn(userToNotify, collectionChangedId);
                if (isOwn != null)
                {
                    var pagesPolicy = await PagesApplyPolicy(isOwn, pages, userToNotify);
                    if (pagesPolicy != null)
                    {
                        var data = new DrawDrawingDTO
                        {
                            Pages = pagesPolicy ?? new List<DrawPageGetDTO>(),
                            Name = await PageCollectionName(collectionChangedId) ?? "",
                            Editable = pagesPolicy != null && pagesPolicy.ElementAt(0).Editable,
                            NewUserMakingChangesName = null
                        };
                        await _hubService.SendMessageToUser(nameof(DrawPages), data, userToNotify, collectionChangedId);
                    }
                }
            }
        }
    }

    public async Task TriggerWebsocketNewUserEditing(Guid collectionChangedId, Guid previousUserId, Guid newUserId)
    {
        string userNameNewUser = (await _dataContext.Users.FirstAsync(usr => usr.Id == newUserId)).Username;
        var pages = await PagesWithoutPolicy(collectionChangedId);
        var isOwn = await PagesIsOwn(previousUserId, collectionChangedId);
        var pagesPolicy = await PagesApplyPolicy(isOwn, pages, previousUserId);
        var data = new DrawDrawingDTO
        {
            Pages = pagesPolicy ?? new List<DrawPageGetDTO>(),
            Name = await PageCollectionName(collectionChangedId) ?? "",
            Editable = pagesPolicy != null && pagesPolicy.ElementAt(0).Editable,
            NewUserMakingChangesName = userNameNewUser
        };
        await _hubService.SendMessageToUser(nameof(DrawPages), data, previousUserId, collectionChangedId);
    }

    public async Task AdjustChangedOnCollection(Guid collectionId, Guid changedById)
    {
        var timeStamp = DateTime.UtcNow;
        var collection = await _dataContext.DrawCollections.FirstAsync(clc => clc.Id == collectionId);
        collection.ChangedById = changedById;
        collection.Changed = timeStamp;
        await _dataContext.SaveChangesAsync();
    }
}