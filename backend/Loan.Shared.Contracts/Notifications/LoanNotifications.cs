using Loan.Shared.Contracts.Abstractions;

namespace Loan.Shared.Contracts.Notifications;

public record LoanDraftAssigned(string DraftId, string LoanId, int LoanStatus) : BaseMessage;

public record LoanApproved(string LoanId, int LoanStatus) : BaseMessage;

public record LoanRejected(string LoanId, int LoanStatus) : BaseMessage;

public record LoanCreated(string LoanId, int LoanStatus) : BaseMessage;

public record LoanCanceled(string LoanId, int LoanStatus) : BaseMessage;

public record LoanStatusUpdated(string LoanId, int LoanStatus) : BaseMessage;

public record LoanSubmitted(string LoanId, int LoanStatus) : BaseMessage;