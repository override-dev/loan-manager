using Bff.Models;

namespace Bff.Interfaces;

internal interface ILoanPublisher
{
    Task PublishLoanSubmittedAsync(LoanSubmissionRequest loanCreationRequest, CancellationToken cancellationToken);
}
