using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Learnz.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class TestGroupAdjustUserPoints : Controller
{
    private readonly DataContext _dataContext;
    private readonly IUserService _userService;
    public TestGroupAdjustUserPoints(DataContext dataContext, IUserService userService)
    {
        _dataContext = dataContext;
        _userService = userService;
    }

}
