using Loan.StorageProvider.Models;

namespace Loan.StorageProvider.Interfaces;

internal interface IStorageProvider
{
    Task<LoanEntity> CreateLoanAsync(LoanEntity loan);
    Task<List<LoanEntity>> GetAllLoansAsync();
    Task<LoanEntity?> GetLoanByIdAsync(string loanId);
    Task<bool> SubmitLoanAsync(string loanId);
    Task<bool> UpdateLoanStatusAsync(string loanId, int newStatus);
}