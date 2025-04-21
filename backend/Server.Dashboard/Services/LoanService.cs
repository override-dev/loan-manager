using Server.Dashboard.Interfaces;
using Server.Dashboard.Models;

namespace Server.Dashboard.Services;

internal class LoanService(IHttpClientFactory httpClientFactory) : ILoanService
{
    public async Task<List<Loan>> GetLoansAsync(CancellationToken ct = default)
    {
        var baseUrl = "https://localhost:7089";
        var client = httpClientFactory.CreateClient();
        var response = await client.GetFromJsonAsync<GetLoansResponse>($"{baseUrl}/loan", ct);

        ArgumentNullException.ThrowIfNull(response, nameof(response));

        return response.Loans;
    }
}
