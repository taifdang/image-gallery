using Application.FileEntries.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Application;

public static class ApplicationServicesExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<IFileService, FileService>();
        
        return services;
    }
}