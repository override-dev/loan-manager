using FastEndpoints;

namespace Server.Loan.Contracts.Features.Loan.Notifications;

public record LoanNotification(string @Event) : IEvent;
