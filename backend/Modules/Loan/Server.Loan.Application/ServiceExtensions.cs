using Microsoft.Extensions.DependencyInjection;
using Server.Loan.Application.Interfaces;
using Server.Loan.Application.Services;

namespace Server.Loan.Application;

internal static class ServiceExtensions
{
    /// <summary>
    /// Adds the application services to the service collection.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <returns>The updated service collection.</returns>
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        // Register message handlers
        services.AddSingleton<IMessageHandlerRegistry, MessageHandlerRegistry>();
      
        return services;
    }
}
