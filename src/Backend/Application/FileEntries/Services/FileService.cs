using Domain.Entities;
using Domain.Repositories;

namespace Application.FileEntries.Services;

public class FileService : IFileService
{
    private readonly IUnitOfWork _unitOfWork;
    protected readonly IRepository<FileEntry, Guid> _repository;

    public FileService(IUnitOfWork unitOfWork, IRepository<FileEntry, Guid> repository)
    {
        _unitOfWork = unitOfWork;
        _repository = repository;
    }
    public async Task AddOrUpdateAsync(FileEntry entity, CancellationToken cancellationToken = default)
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

    public async Task DeleteAsync(FileEntry entity, CancellationToken cancellationToken = default)
    {
        _repository.Delete(entity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }

    public Task<FileEntry> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        if(id == Guid.Empty)
        {
            throw new ArgumentException("Invalid Id");
        }
        return _repository.FirstOrDefaultAsync(_repository.GetQueryableSet().Where(x => x.Id == id));
    }

    public Task<List<FileEntry>> GetAsync(CancellationToken cancellationToken = default)
    {
        return _repository.ToListAsync(_repository.GetQueryableSet().Where(x => !x.Deleted));
    }
}