using Azure.Storage.Queues;
using System.Text.Json;

namespace Infrastructure.Messaging.Azure;
//ref: https://learn.microsoft.com/en-us/azure/storage/queues/storage-dotnet-how-to-use-queues?tabs=dotnet-6-0#queueclient-class
public class AzureQueuePublisher : IEventPublisher
{
    private readonly string _connectionString;
    private readonly string _queueName;

    public AzureQueuePublisher(string connectionString, string queueName)
    {
        _connectionString = connectionString;
        _queueName = queueName;
    }
    public async Task PublishAsync<TEvent>(TEvent @event)
    {
        var _queueClient = new QueueClient(_connectionString, _queueName);
        await _queueClient.CreateIfNotExistsAsync();
        var message = JsonSerializer.Serialize(@event);
        await _queueClient.SendMessageAsync(message);
    }
}