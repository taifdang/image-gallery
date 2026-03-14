using Domain.Entities;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Repositories;

public class FileEntryRepository : Repository<FileEntry, Guid>, IFileEntryRepository
{
    private readonly StorageDbContext _dbContext;

    public FileEntryRepository(StorageDbContext dbContext) : base(dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task DeleteFilesAsync(List<Guid> ids, CancellationToken cancellationToken = default)
    {
        var currentTime = DateTimeOffset.UtcNow;

        await _dbContext.FileEntries
                .Where(x => ids.Contains(x.Id))
                .ExecuteUpdateAsync(setters => setters
                    .SetProperty(x => x.Deleted, true)
                    .SetProperty(x => x.DeletedAt, currentTime));
    }
}
