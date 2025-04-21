using Scalar.AspNetCore;
using FastEndpoints;
using Server.Module.Framework.Extensions;
using Server.Extensions;
using Serilog;


Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateLogger();

var builder = WebApplication.CreateBuilder(args);
builder.AddAzureServiceBusClient(connectionName: "messaging");
builder.AddRedisClient(connectionName: "cache");
builder.Services.AddSerilog(config => config.ReadFrom.Configuration(builder.Configuration));
builder.Services.AddOpenTelemetryServices(builder.Configuration);
builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.InstallAllModules(builder.Configuration);
builder.Configuration.AddEnvironmentVariables(); // we need this to read the environment variables and override the configuration of all the modules
builder.Services.RegisterEndPoints();
builder.Services.AddCors(options =>
{
    if (builder.Environment.IsDevelopment())
    {
        options.AddPolicy("DevelopmentPolicy", builder =>
        {
            builder.AllowAnyOrigin()
                   .AllowAnyHeader()
                   .AllowAnyMethod();
        });
    }
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseOpenApi(c => c.Path = "/openapi/{documentName}.json");
    app.MapScalarApiReference(_ => _.Servers = []);
    app.UseDeveloperExceptionPage();
    app.UseCors("DevelopmentPolicy");
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
app.UseFastEndpoints();

await app.GenerateClientCodeAsync();

app.Run();
