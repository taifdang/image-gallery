//ref: https://github.com/dotnet/aspire/tree/main/src/Aspire.Hosting.Azure.Functions

var builder = DistributedApplication.CreateBuilder(args);

var storage = builder.AddAzureStorage("storage").RunAsEmulator();
var blob = storage.AddBlobs("blob");
var queue = storage.AddQueues("queue");

builder.AddProject<Projects.WebAPI>("webapi");

builder.AddAzureFunctionsProject<Projects.Function>("function")
    .WithReference(blob)
    .WithReference(queue, "QueueTriggerConnection");

builder.Build().Run();
