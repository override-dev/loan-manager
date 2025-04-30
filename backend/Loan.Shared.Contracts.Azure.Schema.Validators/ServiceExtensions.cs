using Loan.Shared.Contract.Abstractions.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Loan.Shared.Contracts.Azure.Schema.Validators;

/// <summary>
/// Extension methods for registering the AzureSchemaValidator service
/// </summary>
public static class ServiceExtensions
{
    /// <summary>
    /// Adds the AzureSchemaValidator service to the service collection
    /// </summary>
    /// <param name="services">The service collection</param>
    /// <param name="configuration">The configuration to use</param>
    /// <returns>The service collection for chaining</returns>
    public static IServiceCollection AddAzureSchemaValidator(this IServiceCollection services, IConfiguration configuration)
    {
        // Configure and register options
        services.Configure<AzureSchemaValidatorOptions>(
            configuration.GetSection(AzureSchemaValidatorOptions.SectionName));

        // Register the service as singleton (to leverage schema cache)
        services.AddSingleton<ISchemaValidator, AzureSchemaValidator>();

        return services;
    }

    /// <summary>
    /// Adds the AzureSchemaValidator service to the service collection with manual configuration
    /// </summary>
    /// <param name="services">The service collection</param>
    /// <param name="configureOptions">Action to configure the options</param>
    /// <returns>The service collection for chaining</returns>
    public static IServiceCollection AddAzureSchemaValidator(
        this IServiceCollection services,
        Action<AzureSchemaValidatorOptions> configureOptions)
    {
        // Configure and register options
        services.Configure(configureOptions);

        // Register the service as singleton (to leverage schema cache)
        services.AddSingleton<ISchemaValidator, AzureSchemaValidator>();

        return services;
    }
}