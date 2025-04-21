using Loan.StorageProvider.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Loan.StorageProvider;

internal static class ServiceExtensions
{
    public static IServiceCollection AddStorageProvider(this IServiceCollection services)
    {
        services.AddSingleton<IStorageProvider, RedisStorageProvider>();
        return services;
    }
}
