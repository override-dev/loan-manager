using FastEndpoints;
using FastEndpoints.Swagger;
using Server.Module.Framework;

namespace Server.Extensions;

internal static class EndPointsExtensions
{
    public static IServiceCollection RegisterEndPoints(this IServiceCollection services)
    {
        using var serviceProvider = services.BuildServiceProvider();
        var modules = serviceProvider.GetRequiredService<IEnumerable<IModuleInstaller>>().DistinctBy(x => x.Name);

        services.SwaggerDocument(o =>
        {
            o.UseOneOfForPolymorphism = true;
            o.DocumentSettings = s =>
            {
                s.DocumentName = "v1";
                s.Version = "v1";
            };
        }).AddFastEndpoints(o => o.Assemblies = modules.Select(x => x.GetType().Assembly));

        return services;
    }
}
