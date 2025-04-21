using Server.Loan.Domain.Aggregates.Loan.Enums;

namespace Server.Loan.Application.Features.Loan.CreateLoan;

internal record CreateLoanCommandResponse(string LoanId, LoanStatus NewStatus);
