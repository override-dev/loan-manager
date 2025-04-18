using Loan.Shared.Contracts.Abstractions;

namespace Loan.Shared.Contracts.Notifications;

public record LoanSubmitted(string LoanId, int LoanStatus) : BaseMessage;
