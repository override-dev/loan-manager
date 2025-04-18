using System.Text.Json;
using Loan.Shared.Contracts.Commands;
using Microsoft.Extensions.Logging;
using Server.Loan.Application.Interfaces;

namespace Server.Loan.Infrastructure.Services.Handlers;

/// <summary>
/// Handler for loan submission requests
/// </summary>
internal class SubmitLoanRequestHandler(ILogger<SubmitLoanRequestHandler> logger) : IMessageHandler
{
    public async Task HandleAsync(string messageContent, CancellationToken cancellationToken)
    {
        var loanSubmission = JsonSerializer.Deserialize<SubmitLoanRequest>(messageContent);

        if (loanSubmission is null)
        {
            logger.LogError("Failed to deserialize SubmitLoanRequest");
            return;
        }

        logger.LogInformation("Processing loan submission request for ID {LoanId}", loanSubmission.LoanId);

        // Implement the business logic for processing the loan submission
        // This could include saving to a database, calling other services, etc.

        await Task.CompletedTask;
    }
}
