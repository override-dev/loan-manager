using System.Text.Json;
using FastEndpoints;
using Loan.Shared.Contracts.Requests;
using Microsoft.Extensions.Logging;
using Server.Loan.Application.Interfaces;
using Server.Loan.Contracts.Features.Loan.SubmitLoan;
using Server.Loan.Infrastructure.Interfaces;

namespace Server.Loan.Infrastructure.Services.Handlers;

/// <summary>
/// Handler for loan submission requests
/// </summary>
internal class SubmitLoanRequestHandler(ILogger<SubmitLoanRequestHandler> logger, ILoanRepositoryFactory loanRepositoryFactory) : IMessageHandler
{
    public async Task HandleAsync(string messageContent, CancellationToken cancellationToken)
    {
        var loanSubmission = JsonSerializer.Deserialize<LoanSubmissionRequested>(messageContent);

        if (loanSubmission is null)
        {
            logger.LogError("Failed to deserialize SubmitLoanRequest");
            return;
        }

        logger.LogInformation("Processing loan submission request for ID {LoanId}", loanSubmission.LoanId);

        var loanDraftsRepository = loanRepositoryFactory.Create(Enums.StorageType.Draft);
        var loanEntity = await loanDraftsRepository.GetLoanByIdAsync(loanSubmission.LoanId);

        if (loanEntity is null)
        {
            logger.LogError("Loan with ID {LoanId} not found", loanSubmission.LoanId);
            return;
        }

        var command = new SubmitLoanCommand
        {
            LoanAmount = loanEntity.LoanAmount,
            LoanTerm = loanEntity.LoanTerm,
            LoanPurpose = loanEntity.LoanPurpose,
            PersonalInformation = new PersonalInformationDto(
                loanEntity.PersonalInformation.FullName,
                loanEntity.PersonalInformation.Email, 
                loanEntity.PersonalInformation.DateOfBirth),
            BankInformation = new BankInformationDto(
                loanEntity.BankInformation.BankName,
                loanEntity.BankInformation.AccountType,
                loanEntity.BankInformation.AccountNumber),
        };

        await command.ExecuteAsync(cancellationToken);

        // Implement the business logic for processing the loan submission
        // This could include saving to a database, calling other services, etc.

        await Task.CompletedTask;
    }
}
