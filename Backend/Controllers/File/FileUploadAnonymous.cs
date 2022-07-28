using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using System.Net.Http.Headers;

namespace Learnz.Controllers;

[Route("api/[controller]")]
[ApiController]
[AllowAnonymous]
public class FileUploadDownloadAnonymous : Controller
{
    private readonly DataContext _dataContext;
    private readonly IConfiguration _configuration;
    public FileUploadDownloadAnonymous(DataContext dataContext, IConfiguration configuration)
    {
        _dataContext = dataContext;
        _configuration = configuration;
    }

    [HttpGet]
    public async Task<ActionResult> FileDownload(string filePath)
    {
        var file = await _dataContext.FilesAnonymous.FirstOrDefaultAsync(f => f.Path == filePath);
        if (file == null)
        {
            return BadRequest(ErrorKeys.FileNotAccessible);
        }
        string path = file.Path;
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
    public async Task<ActionResult<FilePathDTO>> UploadFileAnonymous()
    {
        try
        {
            IFormFile file = Request.Form.Files[0];
            if (file.Length > 0)
            {
                DateTime timeStamp = DateTime.UtcNow;
                Guid fileId = Guid.NewGuid();

                string fileNameExternal = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                string fileNameInternal = fileId.ToString().Replace("-", "") + "." + fileNameExternal.Split(".")[^1];
                string folderName = _configuration["Files:Folder"];
                string path = Path.Combine(Directory.GetCurrentDirectory(), folderName, fileNameInternal);

                LearnzFileAnonymous dbFile = new LearnzFileAnonymous
                {
                    Id = fileId,
                    Created = timeStamp,
                    FileNameExternal = fileNameExternal,
                    FileNameInternal = fileNameInternal,
                    Path = path
                };

                using (var stream = new FileStream(path, FileMode.Create))
                {
                    file.CopyTo(stream);
                }

                await _dataContext.FilesAnonymous.AddAsync(dbFile);
                await _dataContext.SaveChangesAsync();

                FilePathDTO fileDto = new FilePathDTO
                {
                    Path = dbFile.Path,
                    ExternalFilename = dbFile.FileNameExternal
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
        var file = await _dataContext.FilesAnonymous.FirstOrDefaultAsync(f => f.Path == filePath);
        if (file == null)
        {
            return BadRequest(ErrorKeys.FileNotAccessible);
        }
        try
        {
            System.IO.File.Delete(filePath);
        } catch { }
        return Ok();
    }
}
