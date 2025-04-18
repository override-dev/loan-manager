namespace Loan.Shared.Contracts.Abstractions;

public abstract record BaseMessage
{
    public Guid MessageId { get; set; } = Guid.NewGuid();
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    public string Version { get; set; } = "1.0";
}
