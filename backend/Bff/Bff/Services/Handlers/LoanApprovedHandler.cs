using Bff.Interfaces.Interfaces;
using Loan.Shared.Contracts.Notifications;
using Newtonsoft.Json;
using System.Text.Json;

namespace Bff.Services.Handlers;

public class LoanApprovedHandler(ILogger<LoanApprovedHandler> logger) : IMessageHandler
{
    public Task HandleAsync(string messageContent, CancellationToken cancellationToken)
    {
        try
        {
            var loanApprovedEvent = JsonConvert.DeserializeObject<LoanApproved>(messageContent);

            ArgumentNullException.ThrowIfNull(loanApprovedEvent, nameof(loanApprovedEvent));
            logger.LogInformation("Loan approved {LoanId}", loanApprovedEvent.LoanId);


        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error trying to deserializing message");
        }

        return Task.CompletedTask;
    }
}
