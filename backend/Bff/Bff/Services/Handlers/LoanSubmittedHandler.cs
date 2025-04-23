using Bff.Interfaces.Interfaces;
using Loan.Shared.Contracts.Notifications;
using System.Text.Json;

namespace Bff.Services.Handlers;

public class LoanSubmittedHandler(ILogger<LoanSubmittedHandler> logger) : IMessageHandler
{
    public Task HandleAsync(string messageContent, CancellationToken cancellationToken)
    {
		try
		{
            var loanSubmittedEvent = JsonSerializer.Deserialize<LoanSubmitted>(messageContent);

            ArgumentNullException.ThrowIfNull(loanSubmittedEvent, nameof(loanSubmittedEvent));
            logger.LogInformation("Loan submitted {LoanId}", loanSubmittedEvent.LoanId);


        }
        catch (Exception ex)
		{
            logger.LogError(ex, "Error trying to deserializing message");
		}

        return Task.CompletedTask;
    }
}
