using Infrastructure.Messaging.Azure;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Messaging;

public static class MessagingCollectionExtensions
{
    public static IServiceCollection AddMessaging(this IServiceCollection services, MessagingOptions options)
    {
        if (options.UseAzureQueue())
        {
            services.AddAzureQueuePublisher(options.AzureQueue);
        }

        return services;
    }

    public static IServiceCollection AddAzureQueuePublisher(this IServiceCollection services, AzureQueueOption options)
    {
        services.AddSingleton<IEventPublisher>(new AzureQueuePublisher(options.ConnectionString, options.QueueName));

        return services;
    }
}