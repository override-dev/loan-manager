using Azure.Messaging.ServiceBus;
using Bff.Interfaces;
using Loan.Shared.Contracts.Models;
using Loan.Shared.Contracts.Requests;
using Newtonsoft.Json;

namespace Bff.Services;

internal class LoanPublisher(ServiceBusClient mainBusClient) : ILoanPublisher
{
    public async Task PublishLoanSubmittedAsync(LoanSubmissionRequested command, CancellationToken cancellationToken)
    {
        var commandJson = JsonConvert.SerializeObject(command);
        var envelop = new MessageEnvelope(nameof(LoanSubmissionRequested), commandJson);
        var json = JsonConvert.SerializeObject(envelop);
        await mainBusClient
             .CreateSender(Loan.Shared.Contracts.Constants.Topics.LoanQueueName)
             .SendMessageAsync(new ServiceBusMessage(json), cancellationToken);
    }
}
