using System.Text.Json;
using Ardalis.Result;
using FastEndpoints;
using Loan.StorageProvider.Models;
using Server.Loan.Contracts.Features.Loan.Notifications;
using Server.Loan.Contracts.Features.Loan.SubmitLoan;
using Server.Loan.Infrastructure.Interfaces;

namespace Server.Loan.Infrastructure.Integrations;

internal class SubmitLoanCommandHandler(ILoanRepository loanRepository) : CommandHandler<SubmitLoanCommand, Result<SubmitLoanCommandResponse>>
{
    public override async Task<Result<SubmitLoanCommandResponse>> ExecuteAsync(SubmitLoanCommand command, CancellationToken ct = default)
    {

        // 1.- we validate the command using our domain model 

        var personalInformationResult = Domain.Aggregates.Loan.Entities.PersonalInformation.Create(
            fullName: command.PersonalInformation.FullName,
            email: command.PersonalInformation.Email,
            dateOfBirth: command.PersonalInformation.DateOfBirth
        );


        var bankInformationResult = Domain.Aggregates.Loan.Entities.BankInformation.Create(
            accountNumber: command.BankInformation.AccountNumber,
            accountType: command.BankInformation.AccountType,
            bankName: command.BankInformation.BankName
        );

        // 2.- if any entity is not valid we map and return the validations produced by them
        if(!personalInformationResult.IsSuccess)
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
            Id: new Domain.Aggregates.Loan.ValueObjects.LoanId(Guid.NewGuid()),
            LoanAmount: command.LoanAmount,
            LoanTerm: command.LoanTerm,
            LoanPurpose: command.LoanPurpose,
            loanStatus: Domain.Aggregates.Loan.Enums.LoanStatus.Pending,
            personalInformation: personalInformation,
            bankInformation: bankInformation);

        // 4.- if the loan is not valid we map and return the validations produced by it
        if (!loanResult.IsSuccess)
        {
            return loanResult.Map();
        }

        // 5.- we save the loan using the repository
        var loan = loanResult.Value;

        var loanEntity = new LoanEntity
        {
            LoanId = loan.Id.ToString(),
            LoanAmount = loan.LoanAmount,
            LoanTerm = loan.LoanTerm,
            LoanPurpose = loan.LoanPurpose,
            PersonalInformation = new PersonalInformationEntity(loan.PersonalInformation.FullName,
                                                                            loan.PersonalInformation.Email,
                                                                            loan.PersonalInformation.DateOfBirth),
            BankInformation = new BankInformationEntity(loan.BankInformation.AccountNumber,
                                                                    loan.BankInformation.AccountType,
                                                                    loan.BankInformation.BankName),
            LoanStatus = (int)loan.LoanStatus
        };

        var createdLoan = await loanRepository.CreateLoanAsync(loanEntity);

        await loanRepository.SubmitLoanAsync(createdLoan.LoanId);

        // 6.- we publish the events produced by the domain

        foreach (var @event in loan.DomainEvents)
        {
            var eventNotification = new LoanNotification(JsonSerializer.Serialize(@event));
            await eventNotification.PublishAsync(cancellation: ct);
        }

        // 7.- we return the response

        return new SubmitLoanCommandResponse(loan.Id.Value.ToString());

    }
}
