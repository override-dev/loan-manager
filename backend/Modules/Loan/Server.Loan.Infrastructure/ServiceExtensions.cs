using Loan.Shared.Contracts.DataAnnotation.Validators;
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
        // Register the repository that uses the Redis storage provider
        services.AddKeyedTransient<ILoanRepository, RedisLoanRepository>("loan-database");
        services.AddKeyedTransient<ILoanRepository, RedisLoanDraftsRepository>("loan-drafts");
        services.AddTransient<ILoanRepositoryFactory, LoanRepositoryFactory>();
        services.AddHostedService<LoanNotificationConsumer>();
        services.AddTransient<SubmitLoanRequestHandler>();
        services.AddTransient<IStartupFilter, MessageHandlerRegistrationStartupFilter>();
        services.AddSchemaValidators();
        return services;
    }
}
