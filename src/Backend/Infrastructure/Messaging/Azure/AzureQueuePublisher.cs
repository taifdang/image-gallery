using Azure.Storage.Queues;
using System.Text.Json;

namespace Infrastructure.Messaging.Azure;
//ref: https://learn.microsoft.com/en-us/azure/storage/queues/storage-dotnet-how-to-use-queues?tabs=dotnet-6-0#queueclient-class
public class AzureQueuePublisher : IEventPublisher
{
    private readonly AzureQueueOption _option;
    private readonly QueueClient _queueClient;

    public AzureQueuePublisher(AzureQueueOption option)
    {
        _option = option;
        _queueClient = new QueueClient(_option.ConnectionString, _option.QueueName);
    }
    public async Task PublishAsync<TEvent>(TEvent @event)
    {
        await _queueClient.CreateIfNotExistsAsync();
        await _queueClient.SendMessageAsync(JsonSerializer.Serialize(@event));
    }
}