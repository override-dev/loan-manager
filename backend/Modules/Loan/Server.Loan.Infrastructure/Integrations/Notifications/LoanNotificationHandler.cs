using Azure.Messaging.ServiceBus;
using FastEndpoints;
using Loan.Shared.Contracts.Abstractions;
using Loan.Shared.Contracts.Models;
using Loan.Shared.Contracts.Notifications;
using Microsoft.Extensions.Logging;
using Server.Loan.Contracts.Features.Loan.Notifications;
using Server.Loan.Domain.Aggregates.Loan.DomainEvents;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace Server.Loan.Infrastructure.Integrations.Notifications;

/// <summary>
/// Handles loan notifications by processing domain events and dispatching them to a message bus
/// </summary>
internal class LoanNotificationHandler(ServiceBusClient mainBusClient, ILogger<LoanNotificationHandler> logger) : IEventHandler<LoanNotification>
{

    /// <summary>
    /// Processes an incoming loan notification, maps it to the appropriate message type,
    /// and dispatches it to the service bus
    /// </summary>
    /// <param name="notification">The loan notification to process</param>
    /// <param name="cancellationToken">Cancellation token</param>
    public async Task HandleAsync(LoanNotification notification, CancellationToken cancellationToken)
    {
        try
        {
            // Parse the event as a JSON object
            var eventJson = JsonNode.Parse(notification.Event);

            if (eventJson == null)
            {
                logger.LogWarning("Failed to parse event JSON: {Event}", notification.Event);
                return;
            }

            // Extract the event type and loan ID
            var eventType = eventJson["EventType"]?.GetValue<string>() ?? string.Empty;
            var loanId = eventJson["Id"]?.GetValue<string>() ?? string.Empty;

            if (string.IsNullOrEmpty(eventType) || string.IsNullOrEmpty(loanId))
            {
                logger.LogWarning("Missing event type or loan ID in event: {EventType}, {LoanId}", eventType, loanId);
                return;
            }

            // Create the corresponding message based on the event type
            var message = CreateMessageFromEventType(eventType, loanId);

            // Serialize the message to JSON
            var messageContent = JsonSerializer.Serialize(message);

            // Create the envelope with the message type and its content
            var envelope = new MessageEnvelope(message.GetType().Name, messageContent);

            // Serialize the complete envelope
            var envelopeJson = JsonSerializer.Serialize(envelope);

            // Send the message to the service bus
            await mainBusClient
                .CreateSender("loan-notifications")
                .SendMessageAsync(new ServiceBusMessage(envelopeJson), cancellationToken);

            logger.LogInformation("Successfully processed loan notification: {EventType} for loan {LoanId}", eventType, loanId);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error processing loan notification: {Message}", ex.Message);
            throw;
        }
    }

    /// <summary>
    /// Creates the appropriate message based on the event type
    /// </summary>
    /// <param name="eventType">Type of the event</param>
    /// <param name="loanId">ID of the loan</param>
    /// <returns>A message of the appropriate type</returns>
    private BaseMessage CreateMessageFromEventType(string eventType, string loanId)
    {
        // Determine the loan status based on the event type
        int loanStatus = DetermineLoanStatus(eventType);

        return eventType switch
        {
            nameof(LoanApprovedEvent) => new LoanApproved(loanId, loanStatus),
            nameof(LoanRejectedEvent) => new LoanRejected(loanId, loanStatus),
            nameof(LoanCanceledEvent) => new LoanCanceled(loanId, loanStatus),
            nameof(LoanCreatedEvent) => new LoanCreated(loanId, loanStatus),
            nameof(LoanSubmittedEvent) => new LoanSubmitted(loanId, loanStatus),
            nameof(LoanResetEvent) => new LoanStatusUpdated(loanId, loanStatus),
            _ => new LoanStatusUpdated(loanId, -1) // Unknown event type defaults to status update with unknown status
        };
    }

    /// <summary>
    /// Maps event types to their corresponding loan status values
    /// </summary>
    /// <param name="eventType">Type of the event</param>
    /// <returns>The corresponding loan status code</returns>
    private int DetermineLoanStatus(string eventType) => eventType switch
    {
        nameof(LoanApprovedEvent) => 3, // Approved
        nameof(LoanRejectedEvent) => 4, // Rejected
        nameof(LoanCanceledEvent) => 2, // Canceled
        nameof(LoanCreatedEvent) => 5,  // Created
        nameof(LoanSubmittedEvent) => 1, // Submitted
        nameof(LoanResetEvent) => 0,    // Pending/Reset
        _ => -1 // Unknown status
    };
}