using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Learnz.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class DrawCollections : Controller
{
    private readonly DataContext _dataContext;
    private readonly IUserService _userService;
    public readonly IDrawQueryService _drawQueryService;
    public DrawCollections(DataContext dataContext, IUserService userService, IDrawQueryService drawQueryService)
    {
        _dataContext = dataContext;
        _userService = userService;
        _drawQueryService = drawQueryService;
    }

    [HttpGet]
    public async Task<ActionResult<List<DrawCollectionGetDTO>>> GetCollections()
    {
        var guid = _userService.GetUserGuid();
        return Ok();
    }

    [HttpPost]
    public async Task<ActionResult> UpsertCollection(DrawCollectionUpsertDTO request)
    {
        return Ok();
    }

    [HttpDelete]
    public async Task<ActionResult> DeleteCollection(Guid collectionId)
    {
        return Ok();
    }
}