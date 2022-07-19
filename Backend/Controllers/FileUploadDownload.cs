using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using System.Net.Http.Headers;

namespace Learnz.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class FileUploadDownload : Controller
{
    private readonly DataContext _dataContext;
    private readonly IUserService _userService;
    private readonly IFilePolicyChecker _filePolicyChecker;
    private readonly IConfiguration _configuration;

    public FileUploadDownload(DataContext dataContext, IUserService userService, IFilePolicyChecker filePolicyChecker, IConfiguration configuration)
    {
        _dataContext = dataContext;
        _userService = userService;
        _filePolicyChecker = filePolicyChecker;
        _configuration = configuration;
    }

    [HttpGet]
    public async Task<ActionResult> FileDownload(string filePath)
    {
        var guid = _userService.GetUserGuid();
        var file = await _dataContext.Files.FirstOrDefaultAsync(f => f.ActualVersionPath == filePath);
        if (file == null || !_filePolicyChecker.FileDownloadable(file, guid))
        {
            return BadRequest(ErrorKeys.FileNotAccessible);
        }
        string path = file.ActualVersionPath;
        var memory = new MemoryStream();
        await using (var stream = new FileStream(path, FileMode.Open))
        {
            await stream.CopyToAsync(memory);
        }
        memory.Position = 0;
        var provider = new FileExtensionContentTypeProvider();
        string contentType;
        if (!provider.TryGetContentType(path, out contentType))
        {
            contentType = "application/octet-stream";
        }
        return File(memory, contentType, path);
    }

    [HttpPost]
    public async Task<ActionResult<FilePathDTO>> UploadFile()
    {
        try
        {
            IFormFile file = Request.Form.Files[0];
            if (file.Length > 0)
            {
                DateTime timeStamp = DateTime.UtcNow;
                var guid = _userService.GetUserGuid();
                Guid fileVersionId = Guid.NewGuid();
                Guid fileId = Guid.NewGuid();

                string fileNameExternal = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                string fileNameInternal = fileVersionId.ToString().Replace("-", "") + "." + fileNameExternal.Split(".")[^1];
                string folderName = _configuration["Files:Folder"];
                string path = Path.Combine(Directory.GetCurrentDirectory(), folderName, fileNameInternal);

                LearnzFile dbFile = new LearnzFile
                {
                    Id = fileId,
                    ActualVersionId = fileVersionId,
                    ActualVersionFileNameExternal = fileNameExternal,
                    ActualVersionPath = path,
                    FilePolicy = FilePolicy.Private,
                    OwnerId = guid
                };

                LearnzFileVersion dbVersion = new LearnzFileVersion
                {
                    Id = fileVersionId,
                    Created = timeStamp,
                    CreatedById = guid,
                    FileNameExternal = fileNameExternal,
                    FileNameInternal = fileNameInternal,
                    Path = path,
                    FileId = fileId
                };
                
                using (var stream = new FileStream(path, FileMode.Create))
                {
                    file.CopyTo(stream);
                }

                await _dataContext.Files.AddAsync(dbFile);
                await _dataContext.SaveChangesAsync();
                await _dataContext.FileVersions.AddAsync(dbVersion);
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

    [HttpDelete]
    public async Task<ActionResult> DeleteFile(string filePath)
    {
        var guid = _userService.GetUserGuid();
        var file = await _dataContext.Files.FirstOrDefaultAsync(f => f.ActualVersionPath == filePath);
        if (file == null || !_filePolicyChecker.FileDeletable(file, guid))
        {
            return BadRequest(ErrorKeys.FileNotAccessible);
        }
        try
        {
            System.IO.File.Delete(filePath);
        }
        catch
        {
            // do nothing
        }
        var versions = await _dataContext.FileVersions.Where(lvf => lvf.FileId == file.Id).ToListAsync();
        _dataContext.RemoveRange(versions);
        await _dataContext.SaveChangesAsync();
        _dataContext.Remove(file);
        await _dataContext.SaveChangesAsync();
        return Ok();
    }
}
