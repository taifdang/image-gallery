
using Infrastructure.Storage;

namespace WebAPI.Models;

public class FileEntryModel : IFileEntry
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public long Size { get; set; }
    public DateTimeOffset UploadedAt { get; set; }
    public string FileName { get; set; }
    public string FileLocation { get; set; }
    public FileEntryImageModel FileEntryImage { get; set; }
}

public class FileEntryImageModel
{
    public string ImageLocation { get; set; }
}