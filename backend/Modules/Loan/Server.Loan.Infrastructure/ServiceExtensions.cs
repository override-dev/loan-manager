using Loan.StorageProvider;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Server.Loan.Infrastructure.Interfaces;
using Server.Loan.Infrastructure.Services;
using Server.Loan.Infrastructure.Services.Handlers;

namespace Server.Loan.Infrastructure;

internal static class ServiceExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddMemoryCache();
        services.AddStorageProvider(); 

        // Register the repository that uses the Redis storage provider
        services.AddScoped<ILoanRepository, RedisLoanRepository>();
        services.AddHostedService<LoanNotificationConsumer>();
        services.AddTransient<SubmitLoanRequestHandler>();
        services.AddTransient<IStartupFilter, MessageHandlerRegistrationStartupFilter>();
        return services;
    }
}
