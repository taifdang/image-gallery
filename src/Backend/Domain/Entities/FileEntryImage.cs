namespace Domain.Entities;

public class FileEntryImage : Entity<Guid>
{
    public string ImageLocation {get; set;}
    public Guid FileEntryId {get; set;}
    public FileEntry FileEntry {get; set;}
}