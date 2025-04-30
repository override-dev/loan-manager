using Loan.Shared.Contracts.Abstractions;
using System.ComponentModel.DataAnnotations;

namespace Loan.Shared.Contracts.Notifications;

public record LoanDraftAssigned(
    [Required(AllowEmptyStrings =false)]
    string DraftId,
    [Required(AllowEmptyStrings =false)]
    string LoanId,
    int LoanStatus) : BaseMessage;

public record LoanApproved([Required(AllowEmptyStrings = false)] string LoanId, int LoanStatus) : BaseMessage;

public record LoanRejected([Required(AllowEmptyStrings = false)] string LoanId, int LoanStatus) : BaseMessage;

public record LoanCreated([Required(AllowEmptyStrings = false)] string LoanId, int LoanStatus) : BaseMessage;

public record LoanCanceled([Required(AllowEmptyStrings = false)] string LoanId, int LoanStatus) : BaseMessage;

public record LoanStatusUpdated([Required(AllowEmptyStrings = false)] string LoanId, int LoanStatus) : BaseMessage;

public record LoanSubmitted([Required(AllowEmptyStrings = false)] string LoanId, int LoanStatus) : BaseMessage;