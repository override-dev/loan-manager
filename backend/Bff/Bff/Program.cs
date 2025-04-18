using Bff.Interfaces;
using Bff.Services;
using FastEndpoints;
using FastEndpoints.Swagger;
using Loan.StorageProvider;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.AddAzureServiceBusClient(connectionName: "messaging");
builder.Services.AddControllers();
builder.Services.AddTransient<ILoanPublisher, LoanPublisher>();
builder.Services.AddStorageProvider();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();


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
builder.Services.SwaggerDocument(o =>
{
    o.UseOneOfForPolymorphism = true;
    o.DocumentSettings = s =>
    {
        s.DocumentName = "v1";
        s.Version = "v1";
    };
}).AddFastEndpoints();
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

app.Run();
