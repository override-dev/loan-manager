using Loan.StorageProvider.Models;
using Microsoft.Extensions.Caching.Memory;
using Server.Loan.Domain.Aggregates.Loan.Enums;
using Server.Loan.Infrastructure.Interfaces;

namespace Server.Loan.Infrastructure.Services;

internal class InMemoryLoanRepository : ILoanRepository
{
    private readonly IMemoryCache _memoryCache;
    private const string LOANS_CACHE_KEY = "loans_list";

    public InMemoryLoanRepository(IMemoryCache memoryCache)
    {
        _memoryCache = memoryCache;

        if (!_memoryCache.TryGetValue(LOANS_CACHE_KEY, out _))
        {
            _memoryCache.Set(LOANS_CACHE_KEY, new List<LoanEntity>());
        }
    }

    public Task<List<LoanEntity>> GetAllLoansAsync()
    {
        var loans = _memoryCache.Get<List<LoanEntity>>(LOANS_CACHE_KEY) ?? [];
        return Task.FromResult(loans);
    }

    public Task<LoanEntity?> GetLoanByIdAsync(string loanId)
    {
        var loans = _memoryCache.Get<List<LoanEntity>>(LOANS_CACHE_KEY) ?? [];
        var loan = loans.FirstOrDefault(l => l.LoanId == loanId);
        return Task.FromResult(loan);
    }

    public Task<LoanEntity> CreateLoanAsync(LoanEntity loan)
    {
        var loans = _memoryCache.Get<List<LoanEntity>>(LOANS_CACHE_KEY) ?? [];

        // Check if a loan with the same ID already exists
        if (loans.Any(l => l.LoanId == loan.LoanId))
        {
            throw new InvalidOperationException($"A loan with ID: {loan.LoanId} already exists");
        }

        // Add the new loan with initial status Pending
        loans.Add(loan);

        // Update the cache
        _memoryCache.Set(LOANS_CACHE_KEY, loans);

        return Task.FromResult(loan);
    }

    public async Task<bool> UpdateLoanStatusAsync(string loanId, LoanStatus newStatus)
    {
        var loan = await GetLoanByIdAsync(loanId);

        if (loan == null)
        {
            return false;
        }

        var loans = _memoryCache.Get<List<LoanEntity>>(LOANS_CACHE_KEY) ?? [];

        // Find the index of the loan in the list
        int index = loans.FindIndex(l => l.LoanId == loanId);

        if (index == -1)
        {
            return false;
        }

        // Create a new object to update in the list
        // (we can't directly modify the entity because it's a class with immutable properties)
        var updatedLoan = new LoanEntity
        {
            LoanId = loan.LoanId,
            LoanAmount = loan.LoanAmount,
            LoanTerm = loan.LoanTerm,
            LoanPurpose = loan.LoanPurpose,
            PersonalInformation = loan.PersonalInformation,
            BankInformation = loan.BankInformation
        };

        // Update the loan in the list
        loans[index] = updatedLoan;

        // Update the cache
        _memoryCache.Set(LOANS_CACHE_KEY, loans);

        return true;
    }

    public async Task<bool> SubmitLoanAsync(string loanId)
    {
        // The submit operation changes the status to Submitted
        return await UpdateLoanStatusAsync(loanId, LoanStatus.Submitted);
    }
}
