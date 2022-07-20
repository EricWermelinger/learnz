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
    private readonly IConfiguration _configuration;
    public FileVersions(DataContext dataContext, IUserService userService, IFilePolicyChecker filePolicyChecker, ILearnzFrontendFileGenerator learnzFrontendFileGenerator, IConfiguration configuration)
    {
        _dataContext = dataContext;
        _userService = userService;
        _filePolicyChecker = filePolicyChecker;
        _learnzFrontendFileGenerator = learnzFrontendFileGenerator;
        _configuration = configuration;
    }

    [HttpGet]
    public async Task<ActionResult<List<FileVersionInfoDTO>>> GetFileVersions(string path)
    {
        var guid = _userService.GetUserGuid();
        var file = await _dataContext.Files.FirstOrDefaultAsync(f => f.ActualVersionPath == path);
        if (file == null || !_filePolicyChecker.FileDownloadable(file, guid))
        {
            return BadRequest(ErrorKeys.FileNotAccessible);
        }
        var versions = await _dataContext.FileVersions.Where(lvf => lvf.FileId == file.Id)
                                                      .Include(lvf => lvf.CreatedBy)
                                                      .OrderByDescending(lvf => lvf.Created)
                                                      .ToListAsync();
        var frontendFiles = versions.Select(lvf => new FileVersionInfoDTO
        {
            FrontendFile = _learnzFrontendFileGenerator.FrontendFileFromVersion(lvf),
            Created = lvf.Created,
            CreatedBy = lvf.CreatedBy.Username
        });
        return Ok(frontendFiles);
    }

    [HttpPost]
    public async Task<ActionResult<FilePathDTO>> RevertToVersion(FileRevertDTO request)
    {
        var guid = _userService.GetUserGuid();
        var file = await _dataContext.Files.FirstOrDefaultAsync(f => f.ActualVersionPath == request.FilePath);
        var version = await _dataContext.FileVersions.FirstOrDefaultAsync(lvf => lvf.Path == request.VersionPath);
        if (file == null || version == null || !_filePolicyChecker.FileEditable(file, guid))
        {
            return BadRequest(ErrorKeys.FileNotAccessible);
        }

        Guid newVersionId = Guid.NewGuid();
        string newVersionFileNameInternal = newVersionId.ToString().Replace("-", "") + "." + version.FileNameExternal.Split(".")[^1];
        string folderName = _configuration["Files:Folder"];
        string path = Path.Combine(Directory.GetCurrentDirectory(), folderName, newVersionFileNameInternal);
        string oldPath = Path.Combine(Directory.GetCurrentDirectory(), folderName, version.FileNameInternal);
        System.IO.File.Copy(oldPath, path);
        
        var newVersion = new LearnzFileVersion
        {
            Id = newVersionId,
            FileId = file.Id,
            FileNameExternal = version.FileNameExternal,
            FileNameInternal = newVersionFileNameInternal,
            Path = path,
            Created = DateTime.UtcNow,
            CreatedById = guid
        };

        _dataContext.Add(newVersion);
        await _dataContext.SaveChangesAsync();
        var newVersionFrontend = _learnzFrontendFileGenerator.FrontendFileFromVersion(newVersion);
        return Ok(newVersionFrontend);
    }
}
