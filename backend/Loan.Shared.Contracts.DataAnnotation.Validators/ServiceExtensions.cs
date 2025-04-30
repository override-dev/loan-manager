using Loan.Shared.Contract.Abstractions.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Loan.Shared.Contracts.DataAnnotation.Validators;

public static class ServiceExtensions
{
    public static IServiceCollection AddSchemaValidators(this IServiceCollection services)
    {
        services.AddSingleton<ISchemaValidator, DataAnnotationValidator>();
        return services;
    }
}
