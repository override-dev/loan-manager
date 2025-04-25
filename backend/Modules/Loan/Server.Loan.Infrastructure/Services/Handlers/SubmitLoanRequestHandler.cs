using System.Text.Json;
using FastEndpoints;
using Loan.Shared.Contracts.Requests;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Server.Loan.Application.Features.Loan.CreateLoan;
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
        var draftLoanSubmission = JsonConvert.DeserializeObject<LoanSubmissionRequested>(messageContent);

        if (draftLoanSubmission is null)
        {
            logger.LogError("Failed to deserialize SubmitLoanRequest");
            return;
        }

        logger.LogInformation("Processing loan submission request for ID {LoanId}", draftLoanSubmission.LoanId);

        var loanDraftsRepository = loanRepositoryFactory.Create(Enums.StorageType.Draft);
        var draft = await loanDraftsRepository.GetLoanByIdAsync(draftLoanSubmission.LoanId);

        if (draft is null)
        {
            logger.LogError("Loan with ID {LoanId} not found", draftLoanSubmission.LoanId);
            return;
        }

        var createCommand = new CreateLoanCommand(draft.LoanId);
        var createLoanResult = await createCommand.ExecuteAsync(cancellationToken);

        if(!createLoanResult.IsSuccess)
        {
            // send notification that loan creation failed
            logger.LogError("Failed to create loan with ID {LoanId}", draftLoanSubmission.LoanId);
            return;
        }



        logger.LogInformation("Loan with ID {LoanId} successfully created", draftLoanSubmission.LoanId);
        var createdLoanId = createLoanResult.Value.LoanId;
        var submitLoanCommand = new SubmitLoanCommand(createdLoanId);

        var submitLoanResult = await submitLoanCommand.ExecuteAsync(cancellationToken);
        if (!submitLoanResult.IsSuccess)
        {
            // send notification that loan submission failed
            logger.LogError("Failed to submit loan with ID {LoanId}", draftLoanSubmission.LoanId);
            return;
        }
 
        await Task.CompletedTask;
    }
}
