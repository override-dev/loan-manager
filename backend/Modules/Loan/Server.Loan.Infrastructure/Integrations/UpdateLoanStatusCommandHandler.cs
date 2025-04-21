using System.Text.Json;
using Ardalis.Result;
using FastEndpoints;
using Server.Loan.Contracts.Features.Loan.Notifications;
using Server.Loan.Contracts.Features.Loan.UpdateLoanStatus;
using Server.Loan.Domain.Aggregates.Loan.Enums;
using Server.Loan.Domain.Aggregates.Loan.ValueObjects;
using Server.Loan.Domain.Constants;
using Server.Loan.Infrastructure.Interfaces;

namespace Server.Loan.Infrastructure.Integrations;

internal class UpdateLoanStatusCommandHandler(ILoanRepositoryFactory loanRepositoryFactory) : CommandHandler<UpdateLoanStatusCommand, Result<UpdateLoanStatusCommandResponse>>
{
    public override async Task<Result<UpdateLoanStatusCommandResponse>> ExecuteAsync(UpdateLoanStatusCommand command, CancellationToken ct = default)
    {
        var loanRepository = loanRepositoryFactory.Create(Enums.StorageType.Database);
        // we get the loan from the storage

        var loanEntity = await loanRepository.GetLoanByIdAsync(command.LoanId);

        if (loanEntity == null)
        {
            return Result.NotFound();
        }


        // we assume the loan is in valid state but we must perform the validations again.

        // 1.- we validate the command using our domain model 

        var personalInformationResult = Domain.Aggregates.Loan.Entities.PersonalInformation.Create(
            fullName: loanEntity.PersonalInformation.FullName,
            email: loanEntity.PersonalInformation.Email,
            dateOfBirth: loanEntity.PersonalInformation.DateOfBirth
        );


        var bankInformationResult = Domain.Aggregates.Loan.Entities.BankInformation.Create(
            accountNumber: loanEntity.BankInformation.AccountNumber,
            accountType: loanEntity.BankInformation.AccountType,
            bankName: loanEntity.BankInformation.BankName
        );

        // 2.- if any entity is not valid we map and return the validations produced by them
        if (!personalInformationResult.IsSuccess)
        {
            return personalInformationResult.Map();
        }

        if (!bankInformationResult.IsSuccess)
        {
            return bankInformationResult.Map();
        }

        var personalInformation = personalInformationResult.Value;
        var bankInformation = bankInformationResult.Value;


        // 3.- we create the loan using the command and the entities created above

        var loanResult = Domain.Aggregates.Loan.Loan.Create(
            Id: new LoanId(new Guid(loanEntity.LoanId)),
            LoanAmount: loanEntity.LoanAmount,
            LoanTerm: loanEntity.LoanTerm,
            LoanPurpose: loanEntity.LoanPurpose,
            loanStatus: (LoanStatus)loanEntity.LoanStatus, // we pass the current loan Status
            personalInformation: personalInformation,
            bankInformation: bankInformation);

        // 4.- if the loan is not valid we map and return the validations produced by it
        if (!loanResult.IsSuccess)
        {
            return loanResult.Map();
        }

        // 5.- we try to change the status to approved

        var loan = loanResult.Value;

        var status = Enum.IsDefined(typeof(LoanStatus), command.NewStatus);
        if (!status)
        {
            return Result.Invalid(new ValidationError(nameof(command.NewStatus), string.Empty, DomainErrors.Loan.LOAN_STATUS_INVALID, ValidationSeverity.Error));
        }

        var newStatus = (LoanStatus)command.NewStatus;

        switch (newStatus)
        {
            case LoanStatus.Approved:
                var approveResult = loan.Approve();
                if (!approveResult.IsSuccess)
                {
                    return approveResult.Map();
                }
                break;

            case LoanStatus.Rejected:

                var rejectResult = loan.Reject();
                if (!rejectResult.IsSuccess)
                {
                    return rejectResult.Map();
                }
                break;

            case LoanStatus.Pending:
                var resetResult = loan.Reset();
                if (!resetResult.IsSuccess)
                {
                    return resetResult.Map();
                }
                break;
            case LoanStatus.Canceled:
                var cancelResult = loan.Cancel();
                if (!cancelResult.IsSuccess)
                {
                    return cancelResult.Map();
                }
                break;
            case LoanStatus.Submitted:
                var submitResult = loan.Submit();
                if (!submitResult.IsSuccess)
                {
                    return submitResult.Map();
                }
                break;
        }


        // 6.- now the loan can be updated and saved in the storage

        await loanRepository.UpdateLoanStatusAsync(loanEntity.LoanId, newStatus);


        // 7.- notify the events produced by the domain model

        foreach (var @event in loan.DomainEvents)
        {
            var eventNotification = new LoanNotification(JsonSerializer.Serialize(@event));
            await eventNotification.PublishAsync(cancellation: ct);
        }

        return new UpdateLoanStatusCommandResponse(loanEntity.LoanId, (int)loan.LoanStatus);
    }
}
