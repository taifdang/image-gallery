namespace Infrastructure.Storage;

public interface IFileStorageManager : IDisposable
{
    string GenerateSignedUrl(IFileEntry fileEntry);
    Task CreateAsync(IFileEntry fileEntry, Stream stream, CancellationToken cancellationToken = default);
    Task CreateAsync(IFileEntry fileEntry, Stream stream, string? contentType, CancellationToken cancellationToken = default);
    Task DeleteAsync(IFileEntry fileEntry, CancellationToken cancellationToken = default);
    Task<byte[]> ReadAsync(IFileEntry fileEntry, CancellationToken cancellationToken = default);
    Task DownloadAsync(IFileEntry fileEntry, Stream stream, CancellationToken cancellationToken = default);
    Task<Stream> DownloadAsync(IFileEntry fileEntry, CancellationToken cancellationToken = default);
}

public interface IFileEntry
{
    public Guid Id { get; set; }
    public string FileName { get; set; }
    public string FileLocation { get; set; }
}