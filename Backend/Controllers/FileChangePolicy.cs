using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Learnz.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class FileChangePolicy : Controller
{
    private readonly DataContext _dataContext;
    private readonly IUserService _userService;
    private readonly IFilePolicyChecker _filePolicyChecker;
    public FileChangePolicy(DataContext dataContext, IUserService userService, IFilePolicyChecker filePolicyChecker)
    {
        _dataContext = dataContext;
        _userService = userService;
        _filePolicyChecker = filePolicyChecker;
    }

    [HttpPost]
    public async Task<ActionResult> ChangePolicy(FileChangePolicyDTO request)
    {
        var guid = _userService.GetUserGuid();
        var file = await _dataContext.Files.FirstOrDefaultAsync(f => f.ActualVersionPath == request.FilePath && _filePolicyChecker.FilePolicyChangeable(f, guid));
        if (file == null)
        {
            return BadRequest(ErrorKeys.FileNotAccessible);
        }
        file.FilePolicy = (FilePolicy)request.Policy;
        await _dataContext.SaveChangesAsync();
        return Ok();
    }

}
