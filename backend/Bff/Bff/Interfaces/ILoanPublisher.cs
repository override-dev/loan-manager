using Loan.Shared.Contracts.Commands;

namespace Bff.Interfaces;

internal interface ILoanPublisher
{
    Task PublishLoanSubmittedAsync(SubmitLoanRequest command, CancellationToken cancellationToken);
}
