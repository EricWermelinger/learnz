using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Learnz.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class GroupFiles : Controller
{
    private readonly DataContext _dataContext;
    private readonly IUserService _userService;
    private readonly IFilePolicyChecker _filePolicyChecker;

    public GroupFiles(DataContext dataContext, IUserService userService, IFilePolicyChecker filePolicyChecker)
    {
        _dataContext = dataContext;
        _userService = userService;
        _filePolicyChecker = filePolicyChecker;
    }

    [HttpGet]
    public async Task<ActionResult<List<FileInfoDTO>>> GetFiles(Guid groupId)
    {
        var guid = _userService.GetUserGuid();
        if (!(await _dataContext.GroupMembers.AnyAsync(gm => gm.GroupId == groupId && gm.UserId == guid)))
        {
            return BadRequest();
        }

        var files = await _dataContext.GroupFiles
            .Where(gf => gf.GroupId == groupId)
            .Select(gf => new FileInfoDTO
            {
                FileId = gf.FileId,
                FileNameExternal = gf.File.FileNameExternal,
                FilePath = gf.File.Path,
                Created = gf.File.Created,
                CreatedUsername = gf.File.CreatedBy.Username,
                Modified = gf.File.Modified,
                ModifiedUsername = gf.File.ModifiedBy.Username,
                FileFromMe = gf.File.CreatedById == guid,
                FileEditable = _filePolicyChecker.FileEditable(gf.File, guid),
                FileDeletable = _filePolicyChecker.FileDeletable(gf.File, guid)
            })
            .ToListAsync();

        return Ok(files);
    }

    [HttpPost]
    public async Task<ActionResult> EditFiles(GroupFilesEditDTO request)
    {
        var guid = _userService.GetUserGuid();
        if (!(await _dataContext.GroupMembers.AnyAsync(gm => gm.GroupId == request.GroupId && gm.UserId == guid)))
        {
            return BadRequest();
        }
        
        var addedFiles = await _dataContext.Files
            .Where(f => request.FilesAdded.Contains(f.Id))
            .ToListAsync();

        var deletedFiles = await _dataContext.Files
            .Where(f => request.FilesDeleted.Contains(f.Id))
            .ToListAsync();

        foreach (LearnzFile file in deletedFiles)
        {
            if (!_filePolicyChecker.FileDeletable(file, guid))
            {
                return BadRequest();
            }
        }
        
        var existingGroupFiles = await _dataContext.GroupFiles
            .Where(gf => gf.GroupId == request.GroupId)
            .ToListAsync();

        foreach (LearnzFile file in deletedFiles)
        {
            _dataContext.GroupFiles.Remove(existingGroupFiles.First(gf => gf.FileId == file.Id));
            _dataContext.Files.Remove(file);
        }

        foreach (LearnzFile file in addedFiles)
        {
            await _dataContext.GroupFiles.AddAsync(new GroupFile
            {
                GroupId = request.GroupId,
                FileId = file.Id
            });
        }

        await _dataContext.SaveChangesAsync();

        return Ok();
    }
}
