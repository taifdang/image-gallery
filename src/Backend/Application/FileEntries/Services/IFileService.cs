
namespace Application.FileEntries.Services;

public interface IFileService<TEntity> where TEntity : Entity<Guid>
{
    Task AddOrUpdateAsync(TEntity entity, CancellationToken cancellationToken = default);
    Task DeleteAsync(TEntity entity, CancellationToken cancellationToken = default);
    Task<TEntity> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<List<TEntity>> GetAsync(CancellationToken cancellationToken = default);
}
