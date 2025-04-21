using Aspire.Hosting;
using Projects;

var builder = DistributedApplication.CreateBuilder(args);

// Add Service Bus with emulator and create topic
var serviceBus = builder.AddAzureServiceBus("messaging")
    .RunAsEmulator();

var loanDrafts = builder.AddRedis("loan-drafts");
var loanDatabase = builder.AddRedis("loan-database");

// Add topic separately for better readability and control
serviceBus.AddServiceBusQueue("loan-notifications");

// Add projects with reference to Service Bus
var server = builder.AddProject<Projects.Server>("server")
    .WithExternalHttpEndpoints()
    .WithReference(serviceBus)
    .WaitFor(serviceBus)
    .WithReference(loanDrafts)
    .WaitFor(loanDrafts)
    .WithReference(loanDatabase)
    .WaitFor(loanDatabase);

builder.AddProject<Projects.Bff>("bff")
    .WithExternalHttpEndpoints()
    .WithReference(serviceBus)
    .WithReference(loanDrafts)
    .WaitFor(loanDrafts);

builder.AddProject<Server_Dashboard>("dashboard")
    .WaitFor(server);
    
builder.Build().Run();