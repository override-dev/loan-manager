using Projects;

var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Server>("server")
    .WithExternalHttpEndpoints();

builder.Build().Run();
