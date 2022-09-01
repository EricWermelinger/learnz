using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Learnz.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class TestCreate : Controller
{
    private readonly DataContext _dataContext;
    private readonly IUserService _userService;
    private readonly ISetPolicyChecker _setPolicyChecker;
    private readonly ITestQueryService _testQueryService;
    public TestCreate(DataContext dataContext, IUserService userService, ISetPolicyChecker setPolicyChecker, ITestQueryService testQueryService)
    {
        _dataContext = dataContext;
        _userService = userService;
        _setPolicyChecker = setPolicyChecker;
        _testQueryService = testQueryService;
    }

    [HttpPost]
    public async Task<ActionResult> CreateTest(TestCreateDTO request)
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
        var test = new Test
        {
            Id = request.TestId,
            Name = request.Name,
            Created = DateTime.UtcNow,
            MaxTime = request.MaxTime,
            OwnerId = guid,
            SetId = request.SetId,
            Visible = true,
            Active = true
        };
        _dataContext.Tests.Add(test);
        await _dataContext.SaveChangesAsync();
        var succes = await _testQueryService.CreateTestQuestions(request.TestId, request.SetId);
        if (!succes)
        {
            return BadRequest(ErrorKeys.TestNotAccessible);
        }
        return Ok();
    }
}
