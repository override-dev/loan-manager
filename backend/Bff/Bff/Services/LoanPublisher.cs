using System.Text.Json;
using Azure.Messaging.ServiceBus;
using Bff.Interfaces;
using Loan.Shared.Contracts.Commands;
using Loan.Shared.Contracts.Models;

namespace Bff.Services;

internal class LoanPublisher(ServiceBusClient mainBusClient) : ILoanPublisher
{
    public async Task PublishLoanSubmittedAsync(SubmitLoanRequest command, CancellationToken cancellationToken)
    {
        var commandJson = JsonSerializer.Serialize(command);
        var envelop = new MessageEnvelope(nameof(SubmitLoanRequest), commandJson);
        var json = JsonSerializer.Serialize(envelop);
        await mainBusClient
             .CreateSender("loan-notifications")
             .SendMessageAsync(new ServiceBusMessage(json), cancellationToken);
    }
}
