using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Server.Module.Framework.Extensions;
public static class ModuleInstallerExtensions
{
    /// <summary>
    /// Registers all modules that implement IModuleInstaller in the specified assemblies
    /// </summary>
    public static IServiceCollection InstallModules(
        this IServiceCollection services,
        IConfiguration configuration,
        params Assembly[] assemblies)
    {
        // If no assemblies are specified, use the calling assembly
        if (assemblies == null || assemblies.Length == 0)
        {
            assemblies = [Assembly.GetCallingAssembly()];
        }
        // Register all IModuleInstaller implementers as singletons using Scrutor
        services.Scan(scan => scan
            .FromAssemblies(assemblies)
            .AddClasses(classes => classes.AssignableTo<IModuleInstaller>())
            .AsSelfWithInterfaces()
            .WithSingletonLifetime());
        // Get all instances of IModuleInstaller
        var serviceProvider = services.BuildServiceProvider();
        var moduleInstallers = serviceProvider
            .GetServices<IModuleInstaller>()
            .ToList();
        // Install each module
        foreach (var installer in moduleInstallers)
        {
            installer.Install(services, configuration);
        }
        // Register the module catalog for later use
        services.AddSingleton<IReadOnlyCollection<IModuleInstaller>>(moduleInstallers);
        return services;
    }
    /// <summary>
    /// Registers all modules that implement IModuleInstaller from all loaded assemblies
    /// </summary>
    public static IServiceCollection InstallAllModules(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        return services.InstallModules(
            configuration,
            AppDomain.CurrentDomain.GetAssemblies());
    }
    /// <summary>
    /// Configures all installed modules
    /// </summary>
    public static IApplicationBuilder UseModules(
        this IApplicationBuilder app,
        IConfiguration configuration)
    {
        var installers = app.ApplicationServices
            .GetRequiredService<IReadOnlyCollection<IModuleInstaller>>();
        foreach (var installer in installers)
        {
            installer.Use(app, configuration);
        }
        return app;
    }
}