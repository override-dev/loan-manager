using Bff.Interfaces.Interfaces;

namespace Bff.Services;

/// <summary>
/// Implementation of the message handler registry
/// </summary>
internal class MessageHandlerRegistry : IMessageHandlerRegistry
{
    private readonly Dictionary<string, IMessageHandler> _handlers = new();

    public IMessageHandler? GetHandler(string messageType)
    {
        return _handlers.TryGetValue(messageType, out var handler) ? handler : null;
    }

    public void RegisterHandler(string messageType, IMessageHandler handler)
    {
        _handlers[messageType] = handler;
    }
}
