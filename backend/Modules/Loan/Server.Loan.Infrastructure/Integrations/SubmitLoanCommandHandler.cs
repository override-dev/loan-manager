using System.Text.Json;
using Ardalis.Result;
using FastEndpoints;
using Loan.StorageProvider.Models;
using Server.Loan.Contracts.Features.Loan.Notifications;
using Server.Loan.Contracts.Features.Loan.SubmitLoan;
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
        var personalInformationResult = Domain.Aggregates.Loan.Entities.PersonalInformation.Create(
            fullName: command.PersonalInformation.FullName,
            email: command.PersonalInformation.Email,
            dateOfBirth: command.PersonalInformation.DateOfBirth
        );

        // Validate bank information
        var bankInformationResult = Domain.Aggregates.Loan.Entities.BankInformation.Create(
            accountNumber: command.BankInformation.AccountNumber,
            accountType: command.BankInformation.AccountType,
            bankName: command.BankInformation.BankName
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
            LoanAmount: command.LoanAmount,
            LoanTerm: command.LoanTerm,
            LoanPurpose: command.LoanPurpose,
            loanStatus: Domain.Aggregates.Loan.Enums.LoanStatus.Pending,
            personalInformation: personalInformation,
            bankInformation: bankInformation);

        // STEP 4: Return early if loan creation fails
        if (!loanResult.IsSuccess)
        {
            return loanResult.Map();
        }

        // STEP 5: Persist loan to database
        var loan = loanResult.Value;

        // Trigger domain logic for loan creation
        var loanCreationResult = loan.CreateNewLoan();
        if (!loanCreationResult.IsSuccess)
        {
            return loanCreationResult.Map();
        }

        // Map domain model to persistence entity
        var loanEntity = new LoanEntity
        {
            LoanId = loan.Id.ToString(),
            LoanAmount = loan.LoanAmount,
            LoanTerm = loan.LoanTerm,
            LoanPurpose = loan.LoanPurpose,
            PersonalInformation = new PersonalInformationEntity(
                loan.PersonalInformation.FullName,
                loan.PersonalInformation.Email,
                loan.PersonalInformation.DateOfBirth),
            BankInformation = new BankInformationEntity(
                loan.BankInformation.BankName,
                loan.BankInformation.AccountType,
                loan.BankInformation.AccountNumber),
            LoanStatus = (int)loan.LoanStatus
        };

        // Save to repository
        var loanRepository = loanRepositoryFactory.Create(StorageType.Database);
        var createdLoan = await loanRepository.CreateLoanAsync(loanEntity);

        // after the loan is created, the status is set to pending by reseting the loan status
        var resetLoanStatus = loan.Reset();

        if(!resetLoanStatus.IsSuccess)
        {
            return resetLoanStatus.Map();
        }

        // Submit the loan (changes status)
        var submissionResult = loan.Submit();
        if (!submissionResult.IsSuccess)
        {
            return submissionResult.Map();
        }

        // Update loan status in repository
        await loanRepository.SubmitLoanAsync(createdLoan.LoanId);

        // STEP 6: Publish domain events
        foreach (var @event in loan.DomainEvents)
        {
            var eventNotification = new LoanNotification(JsonSerializer.Serialize(@event));
            await eventNotification.PublishAsync(cancellation: ct);
        }

        // STEP 7: Return success response
        return new SubmitLoanCommandResponse(loan.Id.Value.ToString());
    }
}
