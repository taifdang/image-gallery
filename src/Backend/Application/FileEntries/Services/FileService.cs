using Domain.Repositories;

namespace Application.FileEntries.Services;

public class FileService<TEntity> : IFileService<TEntity> where TEntity : Entity<Guid>
{
    private readonly IUnitOfWork _unitOfWork;
    protected readonly IRepository<TEntity, Guid> _repository;

    public FileService(IUnitOfWork unitOfWork, IRepository<TEntity, Guid> repository)
    {
        _unitOfWork = unitOfWork;
        _repository = repository;
    }
    public async Task AddOrUpdateAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        if (entity.Id.Equals(default))
        {
            await _repository.AddAsync(entity, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
        else
        {
            await _repository.UpdateAsync(entity, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }

    public async Task DeleteAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        _repository.Delete(entity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }

    public Task<TEntity> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        if(id == Guid.Empty)
        {
            throw new ArgumentException("Invalid Id");
        }
        return _repository.FirstOrDefaultAsync(_repository.GetQueryableSet().Where(x => x.Id == id));
    }

    public Task<List<TEntity>> GetAsync(CancellationToken cancellationToken = default)
    {
        return _repository.ToListAsync(_repository.GetQueryableSet());
    }
}