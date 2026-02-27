
using Domain.Entities;

namespace WebAPI.Models;

public static class FileEntryModelMapping
{
    public static IEnumerable<FileEntryModel> ToModels(this IEnumerable<FileEntry> fileEntries)
    {
        return fileEntries.Select(x => x.ToModel());
    }
    public static FileEntryModel ToModel(this FileEntry fileEntry)
    {
        if(fileEntry == null)
        {
            return null;
        }

        return new FileEntryModel
        {
            Id = fileEntry.Id,
            Name = fileEntry.Name,
            Size = fileEntry.Size,
            UploadedAt = fileEntry.UploadedAt,
            FileLocation = fileEntry.FileLocation
        };
    }
}