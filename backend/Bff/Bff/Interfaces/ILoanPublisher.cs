using Loan.Shared.Contracts.Requests;

namespace Bff.Interfaces;

internal interface ILoanPublisher
{
    Task PublishLoanSubmittedAsync(LoanSubmissionRequested command, CancellationToken cancellationToken);
}
