using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Learnz.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class FileInfo : Controller
{
    private readonly DataContext _dataContext;
    private readonly IUserService _userService;
    private readonly IFilePolicyChecker _filePolicyChecker;
    public FileInfo(DataContext dataContext, IUserService userService, IFilePolicyChecker filePolicyChecker)
    {
        _dataContext = dataContext;
        _userService = userService;
        _filePolicyChecker = filePolicyChecker;
    }

    [HttpGet]
    public async Task<ActionResult<FileInfoDTO>> GetFileInfo(string filePath)
    {
        var guid = _userService.GetUserGuid();
        var file = await _dataContext.Files.FirstOrDefaultAsync(f => f.Path == filePath && _filePolicyChecker.FileDownloadable(f, guid));
        if (file == null)
        {
            return BadRequest(ErrorKeys.FileNotAccessible);
        }

        var fileDTO = new FileInfoDTO
        {
            FileNameExternal = file.FileNameExternal,
            FilePath = file.Path,
            Created = file.Created,
            CreatedUsername = file.CreatedBy.Username,
            Modified = file.Modified,
            ModifiedUsername = file.ModifiedBy.Username,
            FileFromMe = file.CreatedById == guid,
            FileEditable = _filePolicyChecker.FileEditable(file, guid),
            FileDeletable = _filePolicyChecker.FileDeletable(file, guid)
        };
        return Ok(fileDTO);
    }
}
