using Loan.StorageProvider.Models;

namespace Loan.StorageProvider.Interfaces;

internal interface IStorageProvider
{
    Task<List<LoanEntity>> GetAllLoansAsync();
    Task<LoanEntity?> GetLoanByIdAsync(string loanId);
    Task<LoanEntity> CreateLoanAsync(LoanEntity loan);
    Task<bool> UpdateLoanStatusAsync(string loanId, int newStatus);
    Task<bool> SubmitLoanAsync(string loanId);
}
