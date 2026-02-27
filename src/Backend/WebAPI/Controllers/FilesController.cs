using Application.FileEntries.Services;
using Domain.Entities;
using Infrastructure.Messaging;
using Infrastructure.Storage;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Models;
using System.Text.Json;
using Domain.Repositories;
using System.Net.Mime;
using System.Net;
using WebAPI.ConfigurationOptions;
using Microsoft.Extensions.Options;

namespace WebAPI.Controllers;

[Produces("application/json")]
[Route("api/[controller]")]
[ApiController]
public class FilesController : Controller
{
    private readonly AppSettings _options;
    private readonly IFileStorageManager _fileManager;
    private readonly IFileService<FileEntry> _fileService;
    private readonly IEventPublisher _publisher;
    private readonly IRepository<FileEntryImage, Guid> _fileEntryImageRepository;


    public FilesController(
        IOptions<AppSettings> options,
        IFileStorageManager fileManager,
        IFileService<FileEntry> fileService,
        IEventPublisher publisher,
        IRepository<FileEntryImage, Guid> fileEntryImageRepository)
    {
        _options = options.Value;
        _fileManager = fileManager;
        _fileService = fileService;
        _publisher = publisher;
        _fileEntryImageRepository = fileEntryImageRepository;
    }

    [HttpPost]
    public async Task<ActionResult<FileEntryModel>> Upload([FromForm] UploadFileModel model)
    {
        var fileEntry = new FileEntry
        {
            Name = model.Name,
            Description = model.Description,
            Size = model.FormFile.Length,
            FileName = model.FormFile.FileName,
            UploadedAt = DateTimeOffset.UtcNow,
            Processed = false,
            Deleted = false
        };

        await _fileService.AddOrUpdateAsync(fileEntry);

        fileEntry.FileLocation = DateTime.Now.ToString("yyyy/MM/dd/") + fileEntry.Id;

        var fileEntryDTO = fileEntry.ToModel();

        using var stream = model.FormFile.OpenReadStream();
        await _fileManager.CreateAsync(fileEntryDTO, stream);

        await _fileService.AddOrUpdateAsync(fileEntry);

        var message = JsonSerializer.Serialize(new
        {
            FileEntryId = fileEntry.Id,
            FileLocation = fileEntry.FileLocation
        });

        await _publisher.PublishAsync(message);

        return Ok(fileEntry.ToModel());
    }

    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> Update(Guid id, [FromBody] FileEntryModel model)
    {
        var fileEntry = await _fileService.GetByIdAsync(id);
        if (fileEntry == null)
        {
            return NotFound();
        }

        fileEntry.Name = model.Name;
        fileEntry.Description = model.Description;

        await _fileService.AddOrUpdateAsync(fileEntry);

        return Ok(model);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var fileEntry = await _fileService.GetByIdAsync(id);
        if (fileEntry == null)
        {
            return NotFound();
        }

        fileEntry.Deleted = true;
        fileEntry.DeletedAt = DateTimeOffset.UtcNow;

        await _fileService.AddOrUpdateAsync(fileEntry);

        return Ok();
    }

    [HttpGet("{id}/download")]
    public async Task<IActionResult> Download(Guid id)
    {
        var fileEntry = await _fileService.GetByIdAsync(id);
        if (fileEntry == null)
        {
            return NotFound();
        }

        var content = await _fileManager.ReadAsync(fileEntry.ToModel());
        return File(content, MediaTypeNames.Application.Octet, WebUtility.HtmlEncode(fileEntry.FileName));
    }

    [HttpGet("{id}/downloadimage")]
    public async Task<IActionResult> DownloadImage(Guid id)
    {
        var fileEntry = await _fileService.GetByIdAsync(id);
        if (fileEntry == null)
        {
            return NotFound();
        }

        var fileEntryImage = _fileEntryImageRepository.GetQueryableSet()
        .Where(x => x.FileEntryId == fileEntry.Id)
        .Select(x => new FileEntryImageModel
        {
            ImageLocation = x.ImageLocation
        }).FirstOrDefault();
        
        var stream = System.IO.File.OpenRead(Path.Combine(_options.Storage.TempFolderPath, fileEntryImage.ImageLocation));
        var ext = Path.GetExtension(fileEntryImage.ImageLocation).ToLowerInvariant();

        return File(stream, MediaTypeNames.Application.Octet, WebUtility.HtmlEncode(fileEntry.FileName + ext));
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var fileEntries = await _fileService.GetAsync();
        return Ok(fileEntries.ToModels());
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<IEnumerable<FileEntryModel>>> Get(Guid id)
    {
        var fileEntry = await _fileService.GetByIdAsync(id);

        if (fileEntry == null || fileEntry.Deleted)
        {
            return Ok(null);
        }

        var model = fileEntry.ToModel();

        model.FileEntryImage = _fileEntryImageRepository.GetQueryableSet()
        .Where(x => x.FileEntryId == fileEntry.Id)
        .Select(x => new FileEntryImageModel
        {
            ImageLocation = x.ImageLocation
        }).FirstOrDefault();

        return Ok(model);
    }
}
