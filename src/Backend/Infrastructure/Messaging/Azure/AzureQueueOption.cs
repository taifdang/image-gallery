namespace Infrastructure.Messaging.Azure;

public class AzureQueueOption
{
    public string ConnectionString { get; set; }
    public string QueueName { get; set; }
}
