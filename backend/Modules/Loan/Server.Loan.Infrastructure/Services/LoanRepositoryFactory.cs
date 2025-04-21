using Microsoft.Extensions.DependencyInjection;
using Server.Loan.Infrastructure.Enums;
using Server.Loan.Infrastructure.Interfaces;

namespace Server.Loan.Infrastructure.Services;

internal class LoanRepositoryFactory(IServiceProvider serviceProvider) : ILoanRepositoryFactory
{
    public ILoanRepository Create(StorageType storageType) => storageType switch
    {
        StorageType.Draft => serviceProvider.GetRequiredKeyedService<ILoanRepository>("loan-drafts"),
        StorageType.Database => serviceProvider.GetRequiredKeyedService<ILoanRepository>("loan-database"),
        _ => throw new NotSupportedException($"Storage type {storageType} is not supported."),
    };
}
