using Projects;

var builder = DistributedApplication.CreateBuilder(args);

var serviceBus = builder.AddAzureServiceBus("messaging")
    .RunAsEmulator();

builder.AddProject<Server>("server")
    .WithExternalHttpEndpoints()
    .WithReference(serviceBus);

builder.AddProject<Projects.Bff>("bff")
    .WithExternalHttpEndpoints()
    .WithReference(serviceBus);

builder.Build().Run();
