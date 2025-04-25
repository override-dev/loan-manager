using Loan.StorageProvider.Interfaces;
using Loan.StorageProvider.Models;
using Newtonsoft.Json;
using StackExchange.Redis;
using System.Text.Json;

namespace Loan.StorageProvider;

internal class RedisStorageProvider(IConnectionMultiplexer connectionMux) : IStorageProvider
{
    private readonly IDatabase _database = connectionMux.GetDatabase();
    private const string LOAN_KEY_PREFIX = "loan:";
    private const string LOAN_IDS_SET = "all_loan_ids";

    public async Task<LoanEntity> CreateLoanAsync(LoanEntity loan)
    {
        // Generate a new loan ID if not provided
        if (string.IsNullOrEmpty(loan.LoanId))
        {
            loan.LoanId = Guid.NewGuid().ToString();
        }

        // Set initial status to 0 (pending)
        loan.LoanStatus = 0;

        // Serialize the loan object
        var loanJson = JsonConvert.SerializeObject(loan);

        // Store the loan in Redis
        var loanKey = LOAN_KEY_PREFIX + loan.LoanId;
        await _database.StringSetAsync(loanKey, loanJson);

        // Add the loan ID to the set of all loan IDs
        await _database.SetAddAsync(LOAN_IDS_SET, loan.LoanId);

        return loan;
    }

    public async Task<List<LoanEntity>> GetAllLoansAsync()
    {
        // Get all loan IDs from the set
        var loanIds = await _database.SetMembersAsync(LOAN_IDS_SET);

        if (loanIds.Length == 0)
        {
            return [];
        }

        // Create a list to store the loans
        var loans = new List<LoanEntity>();

        // Retrieve each loan by ID
        foreach (var id in loanIds)
        {
            var loan = await GetLoanByIdAsync(id.ToString());
            if (loan != null)
            {
                loans.Add(loan);
            }
        }

        return loans;
    }

    public async Task<LoanEntity?> GetLoanByIdAsync(string loanId)
    {
        // Get the loan from Redis
        string loanKey = LOAN_KEY_PREFIX + loanId;
        RedisValue loanJson = await _database.StringGetAsync(loanKey);

        // If the loan doesn't exist, return null
        if (loanJson.IsNullOrEmpty)
        {
            return default;
        }

        // Deserialize and return the loan
        return JsonConvert.DeserializeObject<LoanEntity>(loanJson.ToString());
    }

    public async Task<bool> SubmitLoanAsync(string loanId) => await UpdateLoanStatusAsync(loanId, 1);

    public async Task<bool> UpdateLoanStatusAsync(string loanId, int newStatus)
    {
        // Get the loan
        var loan = await GetLoanByIdAsync(loanId);
        if (loan == null)
        {
            return false;
        }

        // Update the status
        loan.LoanStatus = newStatus;

        // Serialize the updated loan
        var loanJson = JsonConvert.SerializeObject(loan);

        // Store the updated loan back in Redis
        string loanKey = LOAN_KEY_PREFIX + loanId;
        await _database.StringSetAsync(loanKey, loanJson);

        return true;
    }
}