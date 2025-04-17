using Projects;

var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Server>("server")
    .WithExternalHttpEndpoints();

builder.AddProject<Projects.Bff>("bff");

builder.Build().Run();
