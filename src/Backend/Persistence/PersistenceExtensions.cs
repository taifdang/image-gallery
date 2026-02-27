using Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Persistence.Repositories;

namespace Persistence;

public static class Extensions
{
    public static IServiceCollection AddPersistence(this IServiceCollection services, string connectionString)
    {
        services.AddDbContext<StorageDbContext>(options =>
        {
            options.UseSqlServer(connectionString);
        });
        
        services.AddRepositories();

        return services;
    }

    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped(typeof(IRepository<,>), typeof(Repository<,>));

        services.AddScoped(typeof(IUnitOfWork), services =>
        {
            return services.GetRequiredService<StorageDbContext>();
        });

        return services;
    }
}