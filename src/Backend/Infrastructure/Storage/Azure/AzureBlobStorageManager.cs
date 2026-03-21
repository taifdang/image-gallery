
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Sas;

namespace Infrastructure.Storage.Azure;

public class AzureBlobStorageManager : IFileStorageManager
{
    private readonly AzureBlobOption _option;
    private readonly BlobContainerClient _container;
    public AzureBlobStorageManager(AzureBlobOption option)
    {
        _option = option;
        _container = new BlobContainerClient(_option.ConnectionString, _option.Container);
    }

    public string GetBlobName(IFileEntry fileEntry)
    {
        return Path.Combine(_option.Path, fileEntry.FileLocation);
    }

    public string GenerateSignedUrl(IFileEntry fileEntry)
    {
        var blobClient = _container.GetBlobClient(GetBlobName(fileEntry));
        var url = blobClient.GenerateSasUri(BlobSasPermissions.Read, DateTimeOffset.UtcNow.AddMinutes(10));

        return url.ToString();
    }

    public async Task CreateAsync(IFileEntry fileEntry, Stream stream, CancellationToken cancellationToken = default)
    {
        await _container.CreateIfNotExistsAsync(cancellationToken: cancellationToken);

        BlobClient blob = _container.GetBlobClient(GetBlobName(fileEntry));
        await blob.UploadAsync(stream, overwrite: true, cancellationToken: cancellationToken);
    }

    public async Task CreateAsync(IFileEntry fileEntry, Stream stream, string? contentType = null, CancellationToken cancellationToken = default)
    {
        await _container.CreateIfNotExistsAsync(cancellationToken: cancellationToken);

        BlobClient blob = _container.GetBlobClient(GetBlobName(fileEntry));

        var options = new BlobUploadOptions();

        if (!string.IsNullOrEmpty(contentType))
        {
            options.HttpHeaders = new BlobHttpHeaders
            {
                ContentType = contentType
            };
        }

        await blob.UploadAsync(stream, options, cancellationToken: cancellationToken);
    }

    public Task DeleteAsync(IFileEntry fileEntry, CancellationToken cancellationToken = default)
    {
        BlobClient blob = _container.GetBlobClient(GetBlobName(fileEntry));
        return blob.DeleteAsync(cancellationToken: cancellationToken);
    }

    public async Task<byte[]> ReadAsync(IFileEntry fileEntry, CancellationToken cancellationToken = default)
    {
        BlobClient blob = _container.GetBlobClient(GetBlobName(fileEntry));
        using var stream = new MemoryStream();
        await blob.DownloadToAsync(stream, cancellationToken);
        return stream.ToArray();
    }

    public async Task DownloadAsync(IFileEntry fileEntry, Stream stream, CancellationToken cancellationToken = default)
    {
        BlobClient blob = _container.GetBlobClient(GetBlobName(fileEntry));
        await blob.DownloadToAsync(stream, cancellationToken);
    }

    public async Task<Stream> DownloadAsync(IFileEntry fileEntry, CancellationToken cancellationToken = default)
    {
        BlobClient blob = _container.GetBlobClient(GetBlobName(fileEntry));
        return await blob.OpenReadAsync(cancellationToken: cancellationToken);
    }

    public void Dispose()
    {
    }
}
