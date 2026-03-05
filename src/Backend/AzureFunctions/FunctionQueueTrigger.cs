using Application.FileEntries.MessageBusEvents;
using Application.FileEntries.Services;
using Azure.Storage.Queues.Models;
using Domain.Entities;
using Infrastructure.Imaging;
using Infrastructure.Storage;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace AzureFunctions;

public class FunctionQueueTrigger
{
    private readonly ILogger<FunctionQueueTrigger> _logger;
    private readonly ImageProcessingService _imageProcessingService;
    private readonly IFileEntryService _fileEntryService;
    private readonly IFileStorageManager _fileManager;

    public FunctionQueueTrigger(
        ILogger<FunctionQueueTrigger> logger,
        ImageProcessingService imageProcessingService,
        IFileEntryService fileEntryService,
        IFileStorageManager fileManager)
    {
        _logger = logger;
        _imageProcessingService = imageProcessingService;
        _fileEntryService = fileEntryService;
        _fileManager = fileManager;
    }

    [Function(nameof(FunctionQueueTrigger))]
    public async Task Run([QueueTrigger("image-processing-queue", Connection = "QueueTriggerConnection")] QueueMessage message)
    {
        await ProcessMessageAsync(message);
    }

    public async Task ProcessMessageAsync(QueueMessage message, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("C# Queue trigger function processed: {messageText}", message.MessageText);

        var messageData = JsonSerializer.Deserialize<FileCreatedEvent>(message.MessageText);

        var fileEntry = await _fileEntryService.GetByIdAsync(messageData.FileEntry.Id);

        if (fileEntry == null)
        {
            _logger.LogError("FileEntry with ID {fileEntryId} not found.", messageData.FileEntry.Id);
            return;
        }

        try
        {
            var fileBytes = await _fileManager.ReadAsync(fileEntry.ToModel());

            if (fileBytes == null)
            {
                _logger.LogError("Failed to read file for FileEntry ID: {fileEntryId}", fileEntry.Id);
                return;
            }

            var fileStream = await _imageProcessingService.ResizeAsync(fileBytes);

            fileEntry.FileLocation = DateTime.Now.ToString("yyyy/MM/dd/") + fileEntry.Id;
            fileEntry.Processed = true;

            await _fileManager.CreateAsync(fileEntry.ToModel(), fileStream);

            await _fileEntryService.AddOrUpdateAsync(fileEntry, cancellationToken);

            // delete azure queue message after processing
            // Note: Azure Functions automatically deletes the message from the queue if the function executes successfully.
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error processing image for FileEntry ID: {fileEntryId}", fileEntry.Id);
            throw;
        }
    }
}

public class FileEntryModel : IFileEntry
{
    public Guid Id { get; set; }
    public string FileName { get; set; }
    public string FileLocation { get; set; }
}

public static class FileEntryExtensions
{
    public static FileEntryModel ToModel(this FileEntry fileEntry)
    {
        if (fileEntry == null)
        {
            return null;
        }

        return new FileEntryModel
        {
            Id = fileEntry.Id,
            FileName = fileEntry.FileName,
            FileLocation = fileEntry.FileLocation
        };
    }
}

