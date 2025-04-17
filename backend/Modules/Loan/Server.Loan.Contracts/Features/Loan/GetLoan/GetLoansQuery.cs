using Ardalis.Result;
using FastEndpoints;

namespace Server.Loan.Contracts.Features.Loan.GetLoan;

public record GetLoansQuery:ICommand<Result<GetLoansQueryResponse>>;
