using Server.Loan.Domain.Aggregates.Loan.ValueObjects;
using Server.Loan.Domain.Interfaces;

namespace Server.Loan.Domain.Aggregates.Loan.DomainEvents;

internal abstract class LoanDomainEvent(LoanId loanId) : IDomainEvent
{
    public Guid Id { get; } = Guid.NewGuid();
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
    public string EventType => GetType().Name;
    public LoanId LoanId { get; } = loanId;
}

internal class LoanApprovedEvent(LoanId loanId) : LoanDomainEvent(loanId)
{
}

internal class LoanCanceledEvent(LoanId loanId) : LoanDomainEvent(loanId)
{
}

internal class LoanRejectedEvent (LoanId loanId) : LoanDomainEvent(loanId)
{
}

internal class LoanSubmittedEvent (LoanId loanId) : LoanDomainEvent(loanId)
{
}

internal class LoanResetEvent(LoanId loanId) : LoanDomainEvent(loanId)
{
}

internal class LoanCreatedEvent(LoanId loanId) : LoanDomainEvent(loanId)
{
}