using Microsoft.Extensions.DependencyInjection;
using Server.Loan.Domain.Interfaces;

namespace Server.Loan.Infrastructure
{
    internal static class ServiceExtensions
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            services.AddTransient<ILoanService, LoanService>();
            return services;
        }
    }
}
