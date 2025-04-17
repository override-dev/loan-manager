using Bff.Interfaces;
using Bff.Models;

namespace Bff.Services;

internal class LoanPublisher : ILoanPublisher
{
    Task ILoanPublisher.PublishLoanSubmittedAsync(LoanSubmissionRequest loanCreationRequest, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
