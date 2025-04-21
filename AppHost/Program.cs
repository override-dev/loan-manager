using Aspire.Hosting;

var builder = DistributedApplication.CreateBuilder(args);

// Add Service Bus with emulator and create topic
var serviceBus = builder.AddAzureServiceBus("messaging")
    .RunAsEmulator();

var cache = builder.AddRedis("cache");

// Add topic separately for better readability and control
//serviceBus.AddServiceBusTopic("loan-notifications");
serviceBus.AddServiceBusQueue("loan-notifications");

// Add projects with reference to Service Bus
builder.AddProject<Projects.Server>("server")
    .WithExternalHttpEndpoints()
    .WithReference(serviceBus)
    .WaitFor(serviceBus)
    .WithReference(cache)
    .WaitFor(cache);   

builder.AddProject<Projects.Bff>("bff")
    .WithExternalHttpEndpoints()
    .WithReference(serviceBus)
    .WithReference(cache)
    .WaitFor(cache);

builder.Build().Run();