using Loan.StorageProvider.Interfaces;
using Loan.StorageProvider.Models;
using Server.Loan.Domain.Aggregates.Loan.Enums;
using Server.Loan.Infrastructure.Interfaces;

namespace Server.Loan.Infrastructure.Services;

internal class RedisLoanRepository(IStorageProvider storageProvider) : ILoanRepository
{
    public async Task<LoanEntity> CreateLoanAsync(LoanEntity loan) => await storageProvider.CreateLoanAsync(loan);

    public async Task<List<LoanEntity>> GetAllLoansAsync() => await storageProvider.GetAllLoansAsync();

    public async Task<LoanEntity?> GetLoanByIdAsync(string loanId)=> await storageProvider.GetLoanByIdAsync(loanId);

    public async Task<bool> SubmitLoanAsync(string loanId)=> await storageProvider.SubmitLoanAsync(loanId);

    public async Task<bool> UpdateLoanStatusAsync(string loanId, LoanStatus newStatus) => await storageProvider.UpdateLoanStatusAsync(loanId, (int)newStatus);
}