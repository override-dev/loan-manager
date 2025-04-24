using System.Runtime.InteropServices;
using Aspire.Hosting;
using Aspire.Hosting.Azure;
using Projects;

var builder = DistributedApplication.CreateBuilder(args);

var serviceBus = builder.AddAzureServiceBus("messaging");

// workaround for [this issue](https://github.com/dotnet/aspire/issues/8818)
if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX) && RuntimeInformation.OSArchitecture == Architecture.Arm64)
{
    // Add Service Bus with emulator and create topic
    serviceBus.RunAsEmulator(sb =>
        {
            sb.WithHttpEndpoint(targetPort: 5300, name: "sbhealthendpoint")
                .WithImageTag("1.1.2")
                .WithContainerName("messaging")
                .WithEnvironment("SQL_WAIT_INTERVAL", "1");

            var edge = sb.ApplicationBuilder.Resources.OfType<ContainerResource>()
                .First(resource => resource.Name.EndsWith("-sqledge"));

            var annotation = edge.Annotations.OfType<ContainerImageAnnotation>().First();

            annotation.Image = "mssql/server";
            annotation.Tag = "2022-latest";
        });

    var sbHc = serviceBus.Resource.Annotations.OfType<HealthCheckAnnotation>().First();
    serviceBus.Resource.Annotations.Remove(sbHc);

    serviceBus.WithHttpHealthCheck("/health", 200, "sbhealthendpoint");
}
else
{
    serviceBus.RunAsEmulator();
}

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