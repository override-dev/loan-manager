using System.Text.Json;
using Ardalis.Result;
using FastEndpoints;
using Newtonsoft.Json;
using Server.Loan.Contracts.Features.Loan.Notifications;
using Server.Loan.Contracts.Features.Loan.SubmitLoan;
using Server.Loan.Domain.Aggregates.Loan.Enums;
using Server.Loan.Infrastructure.Enums;
using Server.Loan.Infrastructure.Interfaces;

namespace Server.Loan.Infrastructure.Integrations;

//TODO: implement Saga design pattern
internal class SubmitLoanCommandHandler(ILoanRepositoryFactory loanRepositoryFactory) : CommandHandler<SubmitLoanCommand, Result<SubmitLoanCommandResponse>>
{
    public override async Task<Result<SubmitLoanCommandResponse>> ExecuteAsync(SubmitLoanCommand command, CancellationToken ct = default)
    {
        // STEP 1: Validate entities using domain models
        // Validate personal information

        var loanRepository = loanRepositoryFactory.Create(StorageType.Database);
        var loanEntity = await loanRepository.GetLoanByIdAsync(command.LoanId);

        if (loanEntity is null)
        {
            return Result<SubmitLoanCommandResponse>.NotFound($"Loan with ID {command.LoanId} not found.");
        }

        var personalInformationResult = Domain.Aggregates.Loan.Entities.PersonalInformation.Create(
            fullName: loanEntity.PersonalInformation.FullName,
            email: loanEntity.PersonalInformation.Email,
            dateOfBirth: loanEntity.PersonalInformation.DateOfBirth
        );

        // Validate bank information
        var bankInformationResult = Domain.Aggregates.Loan.Entities.BankInformation.Create(
            accountNumber: loanEntity.BankInformation.AccountNumber,
            accountType: loanEntity.BankInformation.AccountType,
            bankName: loanEntity.BankInformation.BankName
        );

        // STEP 2: Return early if validation fails
        if (!personalInformationResult.IsSuccess)
        {
            return personalInformationResult.Map();
        }

        if (!bankInformationResult.IsSuccess)
        {
            return bankInformationResult.Map();
        }

        // Extract validated entities
        var personalInformation = personalInformationResult.Value;
        var bankInformation = bankInformationResult.Value;

        // STEP 3: Create loan aggregate with validated entities
        var loanResult = Domain.Aggregates.Loan.Loan.Create(
            Id: new Domain.Aggregates.Loan.ValueObjects.LoanId(Guid.NewGuid()),
            LoanAmount: loanEntity.LoanAmount,
            LoanTerm: loanEntity.LoanTerm,
            LoanPurpose: loanEntity.LoanPurpose,
            loanStatus: LoanStatus.Pending,
            personalInformation: personalInformation,
            bankInformation: bankInformation);

        // STEP 4: Return early if loan creation fails
        if (!loanResult.IsSuccess)
        {
            return loanResult.Map();
        }

        // STEP 5: Persist loan to database
        var loan = loanResult.Value;

        // Submit the loan (changes status)
        var submissionResult = loan.Submit();
        if (!submissionResult.IsSuccess)
        {
            return submissionResult.Map();
        }

        // Update loan status in repository
        await loanRepository.SubmitLoanAsync(loanEntity.LoanId);

        // STEP 6: Publish domain events
        foreach (var @event in loan.DomainEvents)
        {
            var eventNotification = new LoanNotification(JsonConvert.SerializeObject(@event));
            await eventNotification.PublishAsync(cancellation: ct);
        }

        // STEP 7: Return success response
        return new SubmitLoanCommandResponse(loan.Id.Value.ToString());
    }
}
