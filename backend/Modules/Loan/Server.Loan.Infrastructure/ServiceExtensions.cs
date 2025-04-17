using Microsoft.Extensions.DependencyInjection;
using Server.Loan.Domain.Interfaces;
using Server.Loan.Infrastructure.Interfaces;
using Server.Loan.Infrastructure.Services;

namespace Server.Loan.Infrastructure;

internal static class ServiceExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddTransient<ILoanService, LoanService>();
        services.AddMemoryCache();
        services.AddSingleton<ILoanRepository, InMemoryLoanRepository>();
        return services;
    }
}
