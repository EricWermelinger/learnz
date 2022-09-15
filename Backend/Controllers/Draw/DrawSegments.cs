using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Learnz.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class DrawSegments : Controller
{
    private readonly DataContext _dataContext;
    private readonly IUserService _userService;
    private readonly IDrawPolicyChecker _drawPolicyChecker;
    public DrawSegments(DataContext dataContext, IUserService userService, IDrawPolicyChecker drawPolicyChecker)
    {
        _dataContext = dataContext;
        _userService = userService;
        _drawPolicyChecker = drawPolicyChecker;
    }

    [HttpGet]
    public async Task<ActionResult<DrawCanvasSegmentsWrapperDTO>> GetSegments(Guid pageId)
    {
        var guid = _userService.GetUserGuid();
        var collection = await _dataContext.DrawPages.Where(dpg => dpg.Id == pageId).Select(dpg => dpg.DrawCollection).FirstOrDefaultAsync();
        if (collection == null)
        {
            return BadRequest(ErrorKeys.DrawNotAccessible);
        }
        if (collection.OwnerId != guid)
        {
            var groupMember = await _dataContext.DrawGroupCollections.Where(dgc => dgc.DrawCollectionId == collection.Id)
                                                                                                                 .SelectMany(dgc => dgc.Group.GroupMembers.Select(grm => new
                                                                                                                 {
                                                                                                                     UserId = grm.UserId,
                                                                                                                     Policy = dgc.DrawGroupPolicy
                                                                                                                 }))
                                                                                                                 .FirstOrDefaultAsync(grm => grm.UserId == guid);
            if (groupMember == null || !_drawPolicyChecker.GroupPageEditable(groupMember.Policy, collection.OwnerId, guid))
            {
                return BadRequest(ErrorKeys.DrawNotAccessible);
            }
        }
        
        var segments = await _dataContext.DrawCanvasStorages.Where(dgs => dgs.DrawPageId == pageId)
                .Select(dgs => new DrawCanvasStorageDTO
                {
                    Id = dgs.Id,
                    Color = dgs.Color,
                    Created = dgs.Created,
                    Deleted = dgs.Deleted,
                    FromPosition = new DrawCanvasStoragePointDTO
                    {
                        Id = dgs.FromPositionId,
                        X = dgs.FromPosition.X,
                        Y = dgs.FromPosition.Y
                    },
                    ToPosition = new DrawCanvasStoragePointDTO
                    {
                        Id = dgs.ToPositionId,
                        X = dgs.ToPosition.X,
                        Y = dgs.ToPosition.Y
                    },
                    Text = dgs.Text
                })
                .ToListAsync();

        var stepperPosition = await _dataContext.DrawPages.Where(dpg => dpg.Id == pageId).Select(dpg => dpg.StepperPosition).FirstAsync();
        var dto = new DrawCanvasSegmentsWrapperDTO
        {
            Segments = segments,
            StepperPosition = stepperPosition
        };

        return Ok(dto);
    }
}