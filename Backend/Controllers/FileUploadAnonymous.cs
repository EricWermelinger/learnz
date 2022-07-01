using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;

namespace Learnz.Controllers;

[Route("api/[controller]")]
[ApiController]
[AllowAnonymous]
public class FileUploadAnonymous : Controller
{
    private readonly DataContext _dataContext;
    private readonly IConfiguration _configuration;
    public FileUploadAnonymous(DataContext dataContext, IConfiguration configuration)
    {
        _dataContext = dataContext;
        _configuration = configuration;
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
                    ExternalFileName = dbFile.FileNameExternal
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
