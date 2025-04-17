using Ardalis.Result;
using FastEndpoints;
using Server.Loan.Contracts.Features.Loan.SubmitLoan;

namespace Server.Loan.EndPoints.Loan.Submit;

internal class SubmitLoanEndPoint:Endpoint<SubmitLoanRequest, SubmitLoanResponse>
{
    public override void Configure()
    {
        Post();
        AllowAnonymous();
        Summary(s =>
        {
            s.Summary = "Submit a loan application";
            s.Description = "Submit a loan application to the system";
            s.Response<SubmitLoanResponse>();
            s.Response(400, "Invalid request");
            s.Response(500, "Internal server error");
        });
    }
    public override async Task HandleAsync(SubmitLoanRequest req, CancellationToken ct)
    {
        var command = new SubmitLoanCommand
        {
            LoanAmount = req.LoanAmount,
            LoanTerm = req.LoanTerm,
            LoanPurpose = req.LoanPurpose,
            BankInformation = new BankInformationDto(req.BankInformation.AccountNumber, req.BankInformation.AccountType, req.BankInformation.BankName),
            PersonalInformation = new PersonalInformationDto(req.PersonalInformation.FullName, req.PersonalInformation.Email, req.PersonalInformation.DateOfBirth)
        };

        var response = await command.ExecuteAsync(ct);

        if (!response.IsSuccess)
        {
            // for now we send error codes to the frontend
            var validationErrors = response.ValidationErrors.Select(e => new ValidationError(e.Identifier, e.ErrorCode)).ToList();
        }

        await SendOkAsync(new SubmitLoanResponse(response.Value.LoanId),ct);

    }
}
