using Server.Loan.Infrastructure.Enums;

namespace Server.Loan.Infrastructure.Interfaces;

internal interface ILoanRepositoryFactory
{
    ILoanRepository Create(StorageType storageType);
}
