namespace Server.Loan.Domain.Interfaces;

internal interface IDomainEvent
{
    Guid Id { get; }
    DateTime OccurredOn { get; }
    string EventType { get; }
}