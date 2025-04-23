namespace Bff.Interfaces.Interfaces;

/// <summary>
/// Generic message handler interface
/// </summary>
internal interface IMessageHandler
{
    Task HandleAsync(string messageContent, CancellationToken cancellationToken);
}
