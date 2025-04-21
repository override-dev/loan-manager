using Ardalis.Result;
using FastEndpoints;

namespace Server.Loan.Contracts.Features.Loan.SubmitLoan;

public record SubmitLoanCommand(string LoanId):ICommand<Result<SubmitLoanCommandResponse>>;


