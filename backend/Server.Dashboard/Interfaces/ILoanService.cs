using Server.Dashboard.Models;

namespace Server.Dashboard.Interfaces
{
    internal interface ILoanService
    {
        Task<List<Loan>> GetLoansAsync(CancellationToken ct = default);
    }

}
