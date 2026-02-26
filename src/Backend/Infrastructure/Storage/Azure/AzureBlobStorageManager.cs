
using Azure.Storage.Blobs;

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
        return _option.Path + fileEntry.FileLocation;
    }
    public async Task CreateAsync(IFileEntry fileEntry, Stream stream, CancellationToken cancellationToken = default)
    {
        await _container.CreateIfNotExistsAsync(cancellationToken: cancellationToken);

        BlobClient blob = _container.GetBlobClient(GetBlobName(fileEntry));
        await blob.UploadAsync(stream, overwrite: true, cancellationToken);
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
}
