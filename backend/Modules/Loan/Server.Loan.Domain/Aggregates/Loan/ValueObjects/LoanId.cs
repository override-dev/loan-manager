namespace Server.Loan.Domain.Aggregates.Loan.ValueObjects;

internal record LoanId(Guid Value)
{
    public static implicit operator Guid(LoanId loanId) => loanId.Value;
    public static implicit operator LoanId(Guid value) => new(value);
}
