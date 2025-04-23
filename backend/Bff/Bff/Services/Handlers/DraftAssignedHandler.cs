using Bff.Interfaces.Interfaces;

namespace Bff.Services.Handlers;

public class DraftAssignedHandler(ILogger<DraftAssignedHandler> logger) : IMessageHandler
{
    public Task HandleAsync(string messageContent, CancellationToken cancellationToken)
    {
        logger.LogInformation("Draft assigned {MessageContent}", messageContent);
        return Task.CompletedTask;
    }
}
