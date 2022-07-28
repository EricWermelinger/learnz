using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using System.Net.Http.Headers;

namespace Learnz.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class FileVersionUploadDownload : Controller
{
    private readonly DataContext _dataContext;
    private readonly IUserService _userService;
    private readonly IFilePolicyChecker _filePolicyChecker;
    private readonly IConfiguration _configuration;
    public FileVersionUploadDownload(DataContext dataContext, IUserService userService, IFilePolicyChecker filePolicyChecker, IConfiguration configuration)
    {
        _dataContext = dataContext;
        _userService = userService;
        _filePolicyChecker = filePolicyChecker;
        _configuration = configuration;
    }

    [HttpGet]
    public async Task<ActionResult> FileVersionDownload(string filePath)
    {
        var guid = _userService.GetUserGuid();
        var version = await _dataContext.FileVersions.Include(lfv => lfv.File).FirstOrDefaultAsync(lfv => lfv.Path == filePath);
        if (version == null || !_filePolicyChecker.FileDownloadable(version.File, guid))
        {
            return BadRequest(ErrorKeys.FileNotAccessible);
        }
        var memory = new MemoryStream();
        await using (var stream = new FileStream(filePath, FileMode.Open))
        {
            await stream.CopyToAsync(memory);
        }
        memory.Position = 0;
        var provider = new FileExtensionContentTypeProvider();
        string contentType;
        if (!provider.TryGetContentType(filePath, out contentType))
        {
            contentType = "application/octet-stream";
        }
        return File(memory, contentType, filePath);
    }

    [HttpPost]
    public async Task<ActionResult<FilePathDTO>> UploadNewVersion()
    {
        try
        {
            IFormFile file = Request.Form.Files[0];
            string formPath = Request.Form["path"];
            if (file.Length > 0 && formPath != null)
            {
                var guid = _userService.GetUserGuid();
                var existingFile = await _dataContext.Files.FirstOrDefaultAsync(lf => lf.ActualVersionPath == formPath);
                if (existingFile == null || !_filePolicyChecker.FileEditable(existingFile, guid))
                {
                    return BadRequest(ErrorKeys.FileNotAccessible);
                }

                Guid fileVersionId = Guid.NewGuid();

                string fileNameExternal = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                string fileNameInternal = fileVersionId.ToString().Replace("-", "") + "." + fileNameExternal.Split(".")[^1];
                string folderName = _configuration["Files:Folder"];
                string path = Path.Combine(Directory.GetCurrentDirectory(), folderName, fileNameInternal);

                LearnzFileVersion dbVersion = new LearnzFileVersion
                {
                    Id = fileVersionId,
                    Created = DateTime.UtcNow,
                    CreatedById = guid,
                    FileNameExternal = fileNameExternal,
                    FileNameInternal = fileNameInternal,
                    Path = path,
                    FileId = existingFile.Id
                };

                using (var stream = new FileStream(path, FileMode.Create))
                {
                    file.CopyTo(stream);
                }

                await _dataContext.FileVersions.AddAsync(dbVersion);
                await _dataContext.SaveChangesAsync();

                var oldFile = await _dataContext.Files.FirstAsync(lf => lf.ActualVersionPath == formPath);
                oldFile.ActualVersionId = fileVersionId;
                oldFile.ActualVersionFileNameExternal = fileNameExternal;
                oldFile.ActualVersionPath = path;

                await _dataContext.SaveChangesAsync();

                FilePathDTO fileDto = new FilePathDTO
                {
                    Path = dbVersion.Path,
                    ExternalFilename = dbVersion.FileNameExternal
                };
                return Ok(fileDto);
            }
            return BadRequest(ErrorKeys.FileNotProvided);
        }
        catch (Exception ex)
        {
            return BadRequest(ErrorKeys.FileUploadUnsuccessful);
        }
    }
}
