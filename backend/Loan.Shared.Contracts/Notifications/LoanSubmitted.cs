using Loan.Shared.Contracts.Abstractions;

namespace Loan.Shared.Contracts.Events;

public record LoanSubmitted(string LoanId, int LoanStatus) : BaseMessage;
