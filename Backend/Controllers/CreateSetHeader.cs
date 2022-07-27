using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Learnz.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class CreateSetHeader : Controller
{
    private readonly DataContext _dataContext;
    private readonly IUserService _userService;
    private readonly ISetPolicyChecker _setPolicyChecker;
    public CreateSetHeader(DataContext dataContext, IUserService userService, ISetPolicyChecker setPolicyChecker)
    {
        _dataContext = dataContext;
        _userService = userService;
        _setPolicyChecker = setPolicyChecker;
    }

    [HttpPost]
    public async Task<ActionResult> UpsertSet(CreateUpsertSetHeaderDTO request)
    {
        var guid = _userService.GetUserGuid();
        var existing = await _dataContext.CreateSets.FirstOrDefaultAsync(crs => crs.Id == request.Id);
        var timestamp = DateTime.UtcNow;
        if (request.SubjectMain == request.SubjectSecond)
        {
            return BadRequest(ErrorKeys.FillFormCorrectly);
        }
        if (existing == null)
        {
            var newSet = new CreateSet
            {
                Id = request.Id,
                Name = request.Name,
                Description = request.Description,
                Created = timestamp,
                CreatedById = guid,
                Modified = timestamp,
                ModifiedById = guid,
                SetPolicy = request.SetPolicy,
                SubjectMain = request.SubjectMain,
                SubjectSecond = request.SubjectSecond
            };
            _dataContext.CreateSets.Add(newSet);
            await _dataContext.SaveChangesAsync();
            return Ok();
        }
        else
        {
            if (!_setPolicyChecker.SetEditable(existing, guid))
            {
                return BadRequest(ErrorKeys.SetNotAccessible);
            }
            existing.Name = request.Name;
            existing.Description = request.Description;
            existing.Modified = timestamp;
            existing.ModifiedById = guid;
            existing.SetPolicy = request.SetPolicy;
            existing.SubjectMain = request.SubjectMain;
            existing.SubjectSecond = request.SubjectSecond;
            await _dataContext.SaveChangesAsync();
            return Ok();
        }
    }
}
