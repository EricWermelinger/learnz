using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Learnz.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class TestPossibleGroups : Controller
{
    private readonly DataContext _dataContext;
    private readonly IUserService _userService;
    public TestPossibleGroups(DataContext dataContext, IUserService userService)
    {
        _dataContext = dataContext;
        _userService = userService;
    }

    [HttpGet]
    public async Task<ActionResult<List<TestPossibleGroupDTO>>> GetPossibleGroups(string? filter)
    {
        var guid = _userService.GetUserGuid();
        var groups = await _dataContext.Groups.Where(grp => grp.GroupMembers.Any(grm => grm.UserId == guid))
                                              .Where(grp => filter == null || filter == "" | grp.Name.ToLower().Contains(filter.ToLower()))
                                              .Select(grp => new TestPossibleGroupDTO
                                              {
                                                  GroupId = grp.Id,
                                                  GroupName = grp.Name
                                              })
                                              .ToListAsync();
        return Ok(groups);
    }
}
