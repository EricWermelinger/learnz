using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Learnz.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class FileUploadDownload : Controller
{
    private readonly DataContext _dataContext;
    private readonly IUserService _userService;
    private readonly IFilePolicyChecker _filePolicyChecker;

    public FileUploadDownload(DataContext dataContext, IUserService userService, IFilePolicyChecker filePolicyChecker)
    {
        _dataContext = dataContext;
        _userService = userService;
        _filePolicyChecker = filePolicyChecker;
    }

    [HttpGet]
    public async Task<ActionResult<FileInfoDTO>> GetFileInfo(Guid fileId)
    {
        var guid = _userService.GetUserGuid();
        var file = await _dataContext.Files.FirstOrDefaultAsync(f => f.Id == fileId);
        if (file == null)
        {
            return BadRequest();
        }

        var fileDTO = new FileInfoDTO
        {
            FileId = file.Id,
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
