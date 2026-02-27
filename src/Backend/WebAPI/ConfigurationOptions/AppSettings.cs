using Infrastructure.Messaging;
using Infrastructure.Storage;

namespace WebAPI.ConfigurationOptions;

public class AppSettings
{
    public ConnectionStrings ConnectionStrings { get; set; }
    public StorageOptions Storage { get; set; }
    public MessagingOptions Messaging { get; set; }
}