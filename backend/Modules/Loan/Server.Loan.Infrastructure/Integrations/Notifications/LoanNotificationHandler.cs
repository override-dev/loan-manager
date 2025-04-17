using FastEndpoints;
using Server.Loan.Contracts.Features.Loan.Notifications;

namespace Server.Loan.Infrastructure.Integrations.Notifications;

internal class LoanNotificationHandler:IEventHandler<LoanNotification>
{
    public Task HandleAsync(LoanNotification notification, CancellationToken cancellationToken)
    {
        // Handle the loan notification here
        // For example, send an email or push notification
        // we could apply outbox pattern here to send the notification to a queue or a service bus

        return Task.CompletedTask;
    }
}
