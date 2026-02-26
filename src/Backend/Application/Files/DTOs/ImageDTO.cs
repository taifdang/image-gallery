namespace Application.Images.DTOs;

public class ImageDTO
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string ContentType { get; set; }
    public long Size { get; set; }
    public DateTimeOffset UploadedAt { get; set; }
}