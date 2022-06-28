using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
    public async Task<ActionResult<FileInfoDTO>> GetFileInfo(string filePath)
    {
        var guid = _userService.GetUserGuid();
        var file = await _dataContext.Files.FirstOrDefaultAsync(f => f.Path == filePath && _filePolicyChecker.FileDownloadable(f, guid));
        if (file == null)
        {
            return BadRequest();
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
                Guid fileId = Guid.NewGuid();
                
                string fileNameExternal = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                string fileNameInternal = fileId.ToString().Replace("-", "") + "." + fileNameExternal.Split(".")[^1];
                string folderName = _configuration["Files:Folder"];
                string path = Path.Combine(Directory.GetCurrentDirectory(), folderName, fileNameInternal);

                LearnzFile dbFile = new LearnzFile
                {
                    Id = fileId,
                    Created = timeStamp,
                    CreatedById = guid,
                    Modified = timeStamp,
                    ModifiedById = guid,
                    FileNameExternal = fileNameExternal,
                    FileNameInternal = fileNameInternal,
                    FilePolicy = FilePolicy.OnlySelf,
                    Path = path
                };
                
                using (var stream = new FileStream(path, FileMode.Create))
                {
                    file.CopyTo(stream);
                }

                await _dataContext.Files.AddAsync(dbFile);
                await _dataContext.SaveChangesAsync();

                FilePathDTO fileDto = new FilePathDTO
                {
                    Path = dbFile.Path
                };
                return Ok(fileDto);
            }
            return BadRequest("noFileProvided");
        }
        catch (Exception ex)
        {
            return BadRequest("fileUploadUnsuccessful");
        }
    }

    [HttpPut]
    public async Task<ActionResult> UpdateFile()
    {
        try
        {
            IFormFile file = Request.Form.Files[0];
            if (file.Length > 0)
            {
                DateTime timeStamp = DateTime.UtcNow;
                var guid = _userService.GetUserGuid();

                string fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');                
                var dbFile = await _dataContext.Files.FirstOrDefaultAsync(f => f.FileNameInternal.ToLower() == fileName.ToLower() && _filePolicyChecker.FileEditable(f, guid));
                if (dbFile == null)
                {
                    return BadRequest("fileNotFound");
                }
                dbFile.Modified = timeStamp;
                dbFile.ModifiedById = guid;

                System.IO.File.Delete(dbFile.Path);
                using (var stream = new FileStream(dbFile.Path, FileMode.Create))
                {
                    file.CopyTo(stream);
                }

                await _dataContext.SaveChangesAsync();
                return Ok();
            }
            return BadRequest("noFileProvided");
        }
        catch (Exception ex)
        {
            return BadRequest("fileUploadUnsuccessful");
        }
    }
}
