using Ardalis.Result;
using FastEndpoints;

namespace Server.Loan.Contracts.Features.Loan.GetLoans;

public record GetLoansQuery:ICommand<Result<GetLoansQueryResponse>>;
