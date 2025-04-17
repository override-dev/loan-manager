using Loan.StorageProvider.Interfaces;
using Loan.StorageProvider.Models;

namespace Loan.StorageProvider;

internal class StorageProvider : IStorageProvider
{
    public Task<bool> SubmitLoanAsync(string loanId)
    {
        throw new NotImplementedException();
    }

    public Task<bool> UpdateLoanStatusAsync(string loanId, int newStatus)
    {
        throw new NotImplementedException();
    }

    Task<LoanEntity> IStorageProvider.CreateLoanAsync(LoanEntity loan)
    {
        throw new NotImplementedException();
    }

    Task<List<LoanEntity>> IStorageProvider.GetAllLoansAsync()
    {
        throw new NotImplementedException();
    }

    Task<LoanEntity?> IStorageProvider.GetLoanByIdAsync(string loanId)
    {
        throw new NotImplementedException();
    }
}
