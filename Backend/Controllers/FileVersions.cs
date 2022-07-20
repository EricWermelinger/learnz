using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Learnz.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class FileVersions : Controller
{
    private readonly DataContext _dataContext;
    private readonly IUserService _userService;
    private readonly IFilePolicyChecker _filePolicyChecker;
    private readonly ILearnzFrontendFileGenerator _learnzFrontendFileGenerator;
    public FileVersions(DataContext dataContext, IUserService userService, IFilePolicyChecker filePolicyChecker, ILearnzFrontendFileGenerator learnzFrontendFileGenerator)
    {
        _dataContext = dataContext;
        _userService = userService;
        _filePolicyChecker = filePolicyChecker;
        _learnzFrontendFileGenerator = learnzFrontendFileGenerator;
    }

    [HttpGet]
    public async Task<ActionResult<List<LearnzFileFrontendDTO>>> GetFileVersions(string path)
    {
        var guid = _userService.GetUserGuid();
        var file = await _dataContext.Files.FirstOrDefaultAsync(f => f.ActualVersionPath == path);
        if (file == null || _filePolicyChecker.FileDownloadable(file, guid))
        {
            return BadRequest(ErrorKeys.FileNotAccessible);
        }
        var versions = await _dataContext.FileVersions.Where(lvf => lvf.FileId == file.Id)
                                                      .OrderByDescending(lvf => lvf.Created)
                                                      .ToListAsync();
        var frontendFiles = versions.Select(lvf => _learnzFrontendFileGenerator.FrontendFileFromVersion(lvf));
        return Ok(frontendFiles);
    }

    [HttpPost]
    public async Task<ActionResult<FilePathDTO>> RevertToVersion(string versionPath, string filePath)
    {
        var guid = _userService.GetUserGuid();
        var file = await _dataContext.Files.FirstOrDefaultAsync(f => f.ActualVersionPath == filePath);
        var version = await _dataContext.FileVersions.FirstOrDefaultAsync(lvf => lvf.Path == versionPath);
        if (file == null ||version == null || _filePolicyChecker.FileDownloadable(file, guid))
        {
            return BadRequest(ErrorKeys.FileNotAccessible);
        }

        // copy file from version and rename to new guid
        // insert new version into version table
        // edit file to new version
    }
}
