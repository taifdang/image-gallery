using Infrastructure.Storage.Azure;

namespace Infrastructure.Storage;

public class StorageOptions
{
    public string Provider { get; set; }
    public string TempFolderPath { get; set; }
    public AzureBlobOption AzureBlob { get; set; }

    public bool UseAzure()
    {
        return Provider == "Azure";
    }
}