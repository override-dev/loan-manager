using Bff.Interfaces;
using Loan.StorageProvider;
using Loan.StorageProvider.Interfaces;
using Loan.StorageProvider.Models;
using StackExchange.Redis;

namespace Bff.Services
{
    internal class LoanDraftStorageProvider([FromKeyedServices("loan-drafts")] IConnectionMultiplexer chatConnectionMux) : ILoanDraftStorageProvider
    {
        private readonly RedisStorageProvider storageProvider = new(chatConnectionMux);

        public Task<LoanEntity> CreateLoanAsync(LoanEntity loan) => storageProvider.CreateLoanAsync(loan);

        public Task<List<LoanEntity>> GetAllLoansAsync() => storageProvider.GetAllLoansAsync();

        public Task<LoanEntity?> GetLoanByIdAsync(string loanId) => storageProvider.GetLoanByIdAsync(loanId);

        public Task<bool> SubmitLoanAsync(string loanId) => storageProvider.SubmitLoanAsync(loanId);

        public Task<bool> UpdateLoanStatusAsync(string loanId, int newStatus) => storageProvider.UpdateLoanStatusAsync(loanId, newStatus);
    }
}
