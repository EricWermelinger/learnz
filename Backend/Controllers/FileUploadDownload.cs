﻿using Microsoft.AspNetCore.Authorization;
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
        var file = await _dataContext.Files.FirstOrDefaultAsync(f => f.Path == filePath && _filePolicyChecker.FileDownloadable(f, guid));
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
                    FilePolicy = FilePolicy.Private,
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

    [HttpPut]
    public async Task<ActionResult> UpdateFile()
    {
        // todo check same filetype
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
                    return BadRequest(ErrorKeys.FileNotFound);
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
            return BadRequest(ErrorKeys.FileNotProvided);
        }
        catch (Exception ex)
        {
            return BadRequest(ErrorKeys.FileUploadUnsuccessful);
        }
    }
}
