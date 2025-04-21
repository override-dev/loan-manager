using Server.Loan.Contracts.Features.Loan.Common;

namespace Server.Loan.Contracts.Features.Loan.GetLoans;

public record GetLoansQueryResponse(List<LoanDto> Loans);
