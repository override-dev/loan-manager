using Loan.Shared.Contracts.Abstractions;

namespace Loan.Shared.Contracts.Requests;

public record LoanSubmissionRequested(string LoanId) : BaseMessage;
