using FastEndpoints;
using FluentValidation.Results;
using Server.Loan.Contracts.Features.Loan.GetLoan;

namespace Server.Loan.EndPoints.Loan.GetLoans;

internal class GetloansEndPoint:EndpointWithoutRequest<GetLoansResponse>
{
    public override void Configure()
    {
        Get("/");
        Group<LoanGroup>();
        AllowAnonymous();
        Summary(s =>
        {
            s.Summary = "Get all loans";
            s.Description = "Get all loans";
            s.Response<GetLoansResponse>(200);
        });
    }
    public override async Task HandleAsync(CancellationToken ct)
    {
        var query = new GetLoansQuery();
        var response = await query.ExecuteAsync(ct: ct);
        if (!response.IsSuccess)
        {
            // for now we send error codes to the frontend
            var validationErrors = response.ValidationErrors.Select(e => new ValidationFailure(e.Identifier, e.ErrorCode));
            ValidationFailures.AddRange(validationErrors);
           ThrowIfAnyErrors();
            return;
        }
        var items = response.Value.Loans.Select(loan => new Loan(
            Id: loan.Id,
            LoanAmount: loan.LoanAmount,
            LoanTerm: loan.LoanTerm,
            LoanPurpose: loan.LoanPurpose,
            LoanStatus: loan.LoanStatus,
            BankAccountNumber: loan.BankAccountNumber,
            BankAccountType: loan.BankAccountType,
            BankName: loan.BankName,
            FullName: loan.FullName,
            Email: loan.Email,
            DateOfBirth: loan.DateOfBirth
        )).ToList();

        await SendOkAsync(new GetLoansResponse(items), cancellation: ct);
    }
}