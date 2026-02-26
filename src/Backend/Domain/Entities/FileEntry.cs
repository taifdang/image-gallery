namespace Domain.Entities;

public class FileEntry : Entity<Guid>
{
    public string Name { get; set; }
    public string Description { get; set; }
    public long Size { get; set; }
    public string FileName { get; set; }
    public string FileLocation { get; set; }
    public DateTimeOffset UploadedAt { get; set; }
    public bool Deleted { get; set; }
    public DateTimeOffset? DeletedAt { get; set; }
    public bool Processed { get; set; }
    public DateTimeOffset? ProcessedAt { get; set; }
}