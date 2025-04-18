var builder = DistributedApplication.CreateBuilder(args);

// Add Service Bus with emulator and create topic
var serviceBus = builder.AddAzureServiceBus("messaging")
    .RunAsEmulator();

// Add topic separately for better readability and control
serviceBus.AddServiceBusQueue("loan-notifications");

// Add projects with reference to Service Bus
builder.AddProject<Projects.Server>("server")
    .WithExternalHttpEndpoints()
    .WithReference(serviceBus)
    .WaitFor(serviceBus);

builder.AddProject<Projects.Bff>("bff")
    .WithExternalHttpEndpoints()
    .WithReference(serviceBus);

builder.Build().Run();