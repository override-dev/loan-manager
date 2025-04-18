using Loan.StorageProvider.Interfaces;
using Loan.StorageProvider.Models;

namespace Loan.StorageProvider;

internal class StorageProvider : IStorageProvider
{
    public Task<LoanEntity> CreateLoanAsync(LoanEntity loan)
    {
        loan.LoanId = Guid.NewGuid().ToString(); // Generate a new loan ID
        loan.LoanStatus = 0; // Set initial status to 0 (e.g., pending)
        return Task.FromResult(loan); // Simulate loan creation
    }

    public Task<List<LoanEntity>> GetAllLoansAsync()
    {
        throw new NotImplementedException();
    }

    public Task<LoanEntity?> GetLoanByIdAsync(string loanId)
    {
        throw new NotImplementedException();
    }

    public Task<bool> SubmitLoanAsync(string loanId)
    {
        throw new NotImplementedException();
    }

    public Task<bool> UpdateLoanStatusAsync(string loanId, int newStatus)
    {
        throw new NotImplementedException();
    }

}
