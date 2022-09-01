using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Learnz.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class TestGroupTestCreate : Controller
{
    private readonly DataContext _dataContext;
    private readonly IUserService _userService;
    private readonly ISetPolicyChecker _setPolicyChecker;
    private readonly ITestQueryService _testQueryService;
    public TestGroupTestCreate(DataContext dataContext, IUserService userService, ISetPolicyChecker setPolicyChecker, ITestQueryService testQueryService)
    {
        _dataContext = dataContext;
        _userService = userService;
        _setPolicyChecker = setPolicyChecker;
        _testQueryService = testQueryService;
    }

    [HttpPost]
    public async Task<ActionResult> CreateTest(TestGroupTestCreateDTO request)
    {
        var guid = _userService.GetUserGuid();
        
        var testExists = await _dataContext.Tests.AnyAsync(tst => tst.Id == request.TestId);
        if (testExists)
        {
            return BadRequest(ErrorKeys.TestNotAccessible);
        }
        
        var set = await _dataContext.CreateSets.FirstOrDefaultAsync(crs => crs.Id == request.SetId);
        if (set == null || !_setPolicyChecker.SetUsable(set, guid))
        {
            return BadRequest(ErrorKeys.SetNotAccessible);
        }

        var group = await _dataContext.Groups.FirstOrDefaultAsync(grp => grp.Id == request.GroupId && grp.GroupMembers.Any(grm => grm.UserId == guid));
        if (group == null)
        {
            return BadRequest(ErrorKeys.GroupNotAccessible);
        }
        
        var test = new Test
        {
            Id = request.TestId,
            Name = request.Name,
            Created = DateTime.UtcNow,
            MaxTime = request.MaxTime,
            OwnerId = guid,
            SetId = request.SetId,
            Visible = false,
            Active = true
        };
        _dataContext.Tests.Add(test);
        await _dataContext.SaveChangesAsync();
        
        var groupTest = new TestGroup
        {
            GroupId = request.GroupId,
            TestId = request.TestId
        };
        _dataContext.TestGroups.Add(groupTest);
        await _dataContext.SaveChangesAsync();

        var succes = await _testQueryService.CreateTestQuestions(request.TestId, request.SetId);
        if (!succes)
        {
            return BadRequest(ErrorKeys.TestNotAccessible);
        }

        return Ok();
    }
}
