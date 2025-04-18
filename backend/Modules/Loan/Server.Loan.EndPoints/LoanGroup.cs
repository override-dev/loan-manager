using FastEndpoints;

namespace Server.Loan.EndPoints;

internal class LoanGroup : Group
{
    public LoanGroup()
    {
        Configure(nameof(Loan).ToLower(), _ =>
        {

        });
    }
}
