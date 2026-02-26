using Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Repositories;

public class Repository<TEntity, TKey> : IRepository<TEntity, TKey> where TEntity : Entity<TKey>
{
    private readonly StorageDbContext _dbContext;
    protected DbSet<TEntity> dbSet => _dbContext.Set<TEntity>();

    public IUnitOfWork UnitOfWork
    {
        get
        {
            return _dbContext;
        }
    }

    public Repository(StorageDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task AddAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        await dbSet.AddAsync(entity, cancellationToken);
    }
    public Task UpdateAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        return Task.CompletedTask;
    }

    public void Delete(TEntity entity)
    {
        dbSet.Remove(entity);
    }

    public IQueryable<TEntity> GetQueryableSet()
    {
        return _dbContext.Set<TEntity>();
    }

    public Task<T> FirstOrDefaultAsync<T>(IQueryable<T> query)
    {
        return query.FirstOrDefaultAsync();
    }

    public Task<T> SingleOrDefaultAsync<T>(IQueryable<T> query)
    {
        return query.SingleOrDefaultAsync();
    }

    public Task<List<T>> ToListAsync<T>(IQueryable<T> query)
    {
        return query.ToListAsync();
    }
}