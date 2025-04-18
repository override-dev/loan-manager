using System.Text.Json;
using Azure.Messaging.ServiceBus;
using Bff.Interfaces;
using Loan.Shared.Contracts.Models;
using Loan.Shared.Contracts.Requests;

namespace Bff.Services;

internal class LoanPublisher(ServiceBusClient mainBusClient) : ILoanPublisher
{
    public async Task PublishLoanSubmittedAsync(LoanSubmissionRequested command, CancellationToken cancellationToken)
    {
        var commandJson = JsonSerializer.Serialize(command);
        var envelop = new MessageEnvelope(nameof(LoanSubmissionRequested), commandJson);
        var json = JsonSerializer.Serialize(envelop);
        await mainBusClient
             .CreateSender("loan-notifications")
             .SendMessageAsync(new ServiceBusMessage(json), cancellationToken);
    }
}
