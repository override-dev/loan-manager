using Server.Loan.Domain.Aggregates.Loan.Enums;
using Server.Loan.Infrastructure.Persistence;

namespace Server.Loan.Infrastructure.Interfaces;

internal interface ILoanRepository
{
    Task<List<LoanEntity>> GetAllLoansAsync();
    Task<LoanEntity?> GetLoanByIdAsync(string loanId);
    Task<LoanEntity> CreateLoanAsync(LoanEntity loan);
    Task<bool> UpdateLoanStatusAsync(string loanId, LoanStatus newStatus);
    Task<bool> SubmitLoanAsync(string loanId);
}
