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
    public readonly IDrawQueryService _drawQueryService;
    public DrawPages(DataContext dataContext, IUserService userService, IDrawQueryService drawQueryService)
    {
        _dataContext = dataContext;
        _userService = userService;
        _drawQueryService = drawQueryService;
    }

    [HttpGet]
    public async Task<ActionResult<List<DrawPageGetDTO>>> GetPages(Guid collectionId)
    {
        return Ok();
    }

    [HttpPost]
    public async Task<ActionResult> CreatePage(DrawPageCreateDTO request)
    {
        return Ok();
    }

    [HttpPut]
    public async Task<ActionResult> EditPage(DrawPageEditDTO request)
    {
        return Ok();
    }

    [HttpDelete]
    public async Task<ActionResult> DeletePage(Guid collectionId, Guid pageId)
    {
        return Ok();
    }
}