using System.Net.Http.Headers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Learnz.Controllers;

[Route("api/[controller]")]
[ApiController]
[AllowAnonymous]
public class FileTest : Controller
{
    // todo remove this controller + mock PDF File

    private readonly DataContext _dataContext;
    private readonly IConfiguration _configuration;

    public FileTest(DataContext dataContext, IConfiguration configuration)
    {
        _dataContext = dataContext;
        _configuration = configuration;
    }

    [HttpPost]
    public async Task<ActionResult<FilePathDTO>> TestFile()
    {
        try
        {
            DateTime timeStamp = DateTime.UtcNow;
            Guid fileId = Guid.NewGuid();

            string physicalPath = @"C:\src\learnz\Backend\Ressources\mock\Statik Kapitel 2 bis 4 (1).pdf";
            string fileNameExternal = physicalPath.Split("\\")[^1];
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

            System.IO.File.Copy(physicalPath, path);

            await _dataContext.FilesAnonymous.AddAsync(dbFile);
            await _dataContext.SaveChangesAsync();

            FilePathDTO fileDto = new FilePathDTO
            {
                Path = dbFile.Path
            };
            return Ok(fileDto);
        }
        catch (Exception ex)
        {
            return BadRequest(ErrorKeys.FileUploadUnsuccessful);
        }
    }
}
