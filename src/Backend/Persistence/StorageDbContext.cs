using Domain.Entities;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Persistence;

public class StorageDbContext : DbContext, IUnitOfWork
{
    public StorageDbContext(DbContextOptions<StorageDbContext> options) : base(options)
    {
    }

    public DbSet<FileEntry> FileEntries => Set<FileEntry>();
    public DbSet<FileEntryImage> FileEntryImages => Set<FileEntryImage>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(StorageDbContext).Assembly);
        
    }

    //ref: https://learn.microsoft.com/en-us/ef/core/saving/concurrency?tabs=data-annotations#resolving-concurrency-conflicts
    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        HandleFileEntriesDeleted();
        try
        {
            return await base.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateConcurrencyException ex)
        {
            foreach (var entry in ex.Entries)
            {
                var databaseValues = await entry.GetDatabaseValuesAsync();

                if (databaseValues == null)
                {
                    throw;
                }

                entry.OriginalValues.SetValues(databaseValues);
            }
            
            return await base.SaveChangesAsync(cancellationToken);
        }
    }

    private void HandleFileEntriesDeleted()
    {
        var entities = ChangeTracker.Entries<FileEntry>();
        foreach (var entry in entities.Where(e => e.State == EntityState.Deleted))
        {
            entry.State = EntityState.Modified;
            entry.Entity.Deleted = true;
            entry.Entity.DeletedAt = DateTimeOffset.UtcNow;
        }
    }
}
