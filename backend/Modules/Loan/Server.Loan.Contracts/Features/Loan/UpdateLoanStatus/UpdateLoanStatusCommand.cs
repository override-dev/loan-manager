using Ardalis.Result;
using FastEndpoints;

namespace Server.Loan.Contracts.Features.Loan.UpdateLoanStatus;


public record UpdateLoanStatusCommand(string LoanId, int NewStatus) : ICommand<Result<UpdateLoanStatusCommandResponse>>;

public record UpdateLoanStatusCommandResponse(string LoanId, int NewLoanStatus);
