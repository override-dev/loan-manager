namespace Server.Loan.Application.Interfaces;

/// <summary>
/// Generic message handler interface
/// </summary>
internal interface IMessageHandler
{
    Task HandleAsync(string messageContent, CancellationToken cancellationToken);
}
