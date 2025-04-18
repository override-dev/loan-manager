using FastEndpoints;

namespace Bff.EndPoints.Loan;

internal class LoanGroup : Group
{
    public LoanGroup()
    {
        Configure(nameof(Loan).ToLower(), _ =>
        {

        });
    }
}
