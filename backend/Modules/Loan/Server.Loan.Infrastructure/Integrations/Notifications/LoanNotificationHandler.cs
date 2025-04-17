using Azure.Messaging.ServiceBus;
using FastEndpoints;
using Microsoft.Extensions.DependencyInjection;
using Server.Loan.Contracts.Features.Loan.Notifications;

namespace Server.Loan.Infrastructure.Integrations.Notifications;

internal class LoanNotificationHandler([FromKeyedServices("messaging")] ServiceBusClient mainBusClient) :IEventHandler<LoanNotification>
{
    public async Task HandleAsync(LoanNotification notification, CancellationToken cancellationToken)
    {
        // Handle the loan notification here
        // For example, send an email or push notification
        // we could apply outbox pattern here to send the notification to a queue or a service bus
        await mainBusClient
            .CreateSender("loan-notifications")
            .SendMessageAsync(new ServiceBusMessage(notification.Event), cancellationToken);
       
    }
}
