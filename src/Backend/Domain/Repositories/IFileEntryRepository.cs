using Domain.Entities;

namespace Domain.Repositories;

public interface IFileEntryRepository : IRepository<FileEntry, Guid>
{
    Task DeleteFilesAsync(List<Guid> ids, CancellationToken cancellationToken = default);
}
