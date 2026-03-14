using Application.FileEntries.Services;
using Domain.Entities;
using Infrastructure.Storage;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Models;
using Domain.Repositories;
using System.Net.Mime;
using System.Net;
using WebAPI.ConfigurationOptions;
using Microsoft.Extensions.Options;
using Domain.Infrastructure.Messaging;
using Application.FileEntries.MessageBusEvents;

namespace WebAPI.Controllers;

[Produces("application/json")]
[Route("api/[controller]")]
[ApiController]
public class FilesController : Controller
{
    private readonly AppSettings _options;
    private readonly IFileStorageManager _fileManager;
    private readonly IFileEntryService _fileEntryService;
    private readonly IMessageBus _messageBus;
    private readonly IRepository<FileEntryImage, Guid> _fileEntryImageRepository;
    private readonly IFileEntryRepository _fileEntryRepository;

    public FilesController(
        IOptions<AppSettings> options,
        IFileStorageManager fileManager,
        IFileEntryService fileService,
        IRepository<FileEntryImage, Guid> fileEntryImageRepository,
        IFileEntryRepository fileEntryRepository,
        IMessageBus messageBus)
    {
        _options = options.Value;
        _fileManager = fileManager;
        _fileEntryService = fileService;
        _fileEntryImageRepository = fileEntryImageRepository;
        _fileEntryRepository = fileEntryRepository;
        _messageBus = messageBus;
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

        await _fileEntryService.AddOrUpdateAsync(fileEntry);

        fileEntry.FileLocation = $"originals/{DateTime.Now.ToString("yyyy/MM/dd/") + fileEntry.Id}";

        var fileEntryDTO = fileEntry.ToModel();

        using var stream = model.FormFile.OpenReadStream();
        await _fileManager.CreateAsync(fileEntryDTO, stream);

        await _fileEntryService.AddOrUpdateAsync(fileEntry);

        var message = new FileCreatedEvent
        {
            FileEntry = fileEntry,
        };

        await _messageBus.SendAsync(message);

        return Ok(fileEntry.ToModel());
    }

    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> Update(Guid id, [FromBody] FileEntryModel model)
    {
        var fileEntry = await _fileEntryService.GetByIdAsync(id);
        if (fileEntry == null)
        {
            return NotFound();
        }

        fileEntry.Name = model.Name;
        fileEntry.Description = model.Description;

        await _fileEntryService.AddOrUpdateAsync(fileEntry);

        return Ok(model);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var fileEntry = await _fileEntryService.GetByIdAsync(id);
        if (fileEntry == null)
        {
            return NotFound();
        }

        await _fileEntryService.DeleteAsync(fileEntry);

        return Ok();
    }

    [HttpDelete("bulkdelete")]
    public async Task<IActionResult> BulkDelete([FromBody] Guid[] ids)
    {
        await _fileEntryRepository.DeleteFilesAsync(ids.ToList());

        return Ok();
    }

    [HttpGet("{id}/download")]
    public async Task<IActionResult> Download(Guid id)
    {
        var fileEntry = await _fileEntryService.GetByIdAsync(id);
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
        var fileEntry = await _fileEntryService.GetByIdAsync(id);
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
    public async Task<IActionResult> Get()
    {
        var fileEntries = await _fileEntryService.GetAsync();
        return Ok(fileEntries.ToModels());
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<IEnumerable<FileEntryModel>>> Get(Guid id)
    {
        var fileEntry = await _fileEntryService.GetByIdAsync(id);

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
