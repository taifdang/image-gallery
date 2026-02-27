
using Domain.Entities;

namespace Application.FileEntries.Services;

public interface IFileService
{
    Task AddOrUpdateAsync(FileEntry entity, CancellationToken cancellationToken = default);
    Task DeleteAsync(FileEntry entity, CancellationToken cancellationToken = default);
    Task<FileEntry> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<List<FileEntry>> GetAsync(CancellationToken cancellationToken = default);
}
