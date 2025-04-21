using Server.Dashboard.Models;

namespace Server.Dashboard.Interfaces
{
    internal interface ILoanService
    {
        Task<List<Loan>> GetLoansAsync(CancellationToken ct = default);

        Task<string> UploadLoanStatusAsync(string loanId, int status, CancellationToken ct = default);
    }

}
