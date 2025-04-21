using FastEndpoints;
using FluentValidation.Results;
using Server.Loan.Contracts.Features.Loan.UpdateLoanStatus;
using Server.Loan.Domain.Aggregates.Loan.Enums;

namespace Server.Loan.EndPoints.Loan.UpdateLoanStatus;

internal class UpdateLoanStatusEndPoint:EndpointWithoutRequest<UpdateLoanStatuResponse>
{
    public override void Configure()
    {
        Post("update-status/{loanId}/{status}");
        Group<LoanGroup>();
        AllowAnonymous();
        Summary(s =>
        {
            s.Summary = "Update status of a loan";
            s.Description = "Update status of a loan";
            s.Response<UpdateLoanStatuResponse>(200);
        });
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var loanId = Route<string>("loanId");
        var newStatus = Route<string>("status");

        if (string.IsNullOrEmpty(loanId))
        {
            ThrowError(new ValidationFailure("LoanId", "Loan Id is invalid"));
        }

        if (string.IsNullOrEmpty(newStatus)) 
        {
            ThrowError(new ValidationFailure("Status", "Loan Status is invalid"));
        }

        if(!int.TryParse(newStatus, out var status) || !Enum.IsDefined(typeof(LoanStatus),status))
        {
            ThrowError(new ValidationFailure("Status", "Loan Status is not supported"));
        }

        var command = new UpdateLoanStatusCommand(loanId,status);

        var response = await command.ExecuteAsync(ct: ct);

        if (!response.IsSuccess)
        {
            // for now we send error codes to the frontend
            var validationErrors = response.ValidationErrors.Select(e => new ValidationFailure(e.Identifier, e.ErrorCode));
            ValidationFailures.AddRange(validationErrors);
            ThrowIfAnyErrors();
            return;
        }

        await SendOkAsync(new UpdateLoanStatuResponse(response.Value.LoanId), ct);
    }
}
