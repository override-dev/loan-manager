using Ardalis.Result;
using FastEndpoints;

namespace Server.Loan.Application.Features.Loan.CreateLoan;

internal record CreateLoanCommand(
   string DraftLoanId) : ICommand<Result<CreateLoanCommandResponse>>;