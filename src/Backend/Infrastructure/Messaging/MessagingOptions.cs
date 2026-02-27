using Infrastructure.Messaging.Azure;

namespace Infrastructure.Messaging;

public class MessagingOptions
{
    public string Provider {get; set; }
    public AzureQueueOption AzureQueue { get; set; }

    public bool UseAzureQueue()
    {
        return Provider == "AzureQueue";
    }
}