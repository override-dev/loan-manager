using Loan.Shared.Contracts.Abstractions;
using System.ComponentModel.DataAnnotations;

namespace Loan.Shared.Contracts.Requests;

public record LoanSubmissionRequested(
    [Required(AllowEmptyStrings = false)]
    string LoanId) : BaseMessage;
