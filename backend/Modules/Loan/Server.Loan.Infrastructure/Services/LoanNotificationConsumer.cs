using System.Text.Json;
using Azure.Messaging.ServiceBus;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Loan.Shared.Contracts.Models;
using Server.Loan.Application.Interfaces;
using Loan.Shared.Contracts.Constants;

namespace Server.Loan.Infrastructure.Services;

/// <summary>
/// Consumes loan-related notifications from Azure Service Bus
/// </summary>
internal sealed class LoanNotificationConsumer(
    IServiceProvider serviceProvider,
    ILogger<LoanNotificationConsumer> logger,
    IMessageHandlerRegistry messageHandlerRegistry) : BackgroundService
{
    private ServiceBusProcessor? _processor;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        // Allow time for other services to initialize
        await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);

        try
        {
            await InitializeProcessorAsync();
            await _processor!.StartProcessingAsync(stoppingToken);

            // Keep the service running until cancellation is requested
            await Task.Delay(Timeout.Infinite, stoppingToken);
        }
        catch (OperationCanceledException)
        {
            logger.LogInformation("Service bus consumer is stopping");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while initializing or processing service bus messages");
            throw;
        }
        finally
        {
            if (_processor is { IsClosed: false })
            {
                await _processor.StopProcessingAsync(stoppingToken);
            }
        }
    }

    private Task InitializeProcessorAsync()
    {
        if (_processor != null)
        {
            return Task.CompletedTask;
        }

        var serviceBusClient = serviceProvider.GetRequiredService<ServiceBusClient>();
        _processor = serviceBusClient.CreateProcessor(queueName:Topics.LoanQueueName);

        _processor.ProcessMessageAsync += ProcessMessageAsync;
        _processor.ProcessErrorAsync += ProcessErrorAsync;

        logger.LogInformation("Service bus processor initialized for queue {QueueName}", "loan-notifications");

        return Task.CompletedTask;
    }

    private async Task ProcessMessageAsync(ProcessMessageEventArgs args)
    {
        var messageId = args.Message.MessageId;
        logger.LogDebug("Processing message {MessageId}", messageId);

        try
        {
            var body = args.Message.Body.ToString();
            var envelope = JsonSerializer.Deserialize<MessageEnvelope>(body);

            if (envelope is null)
            {
                logger.LogError("Failed to deserialize message {MessageId}", messageId);
                await args.AbandonMessageAsync(args.Message);
                return;
            }

            logger.LogInformation("Received message of type {MessageType} with ID {MessageId}",
                envelope.MessageType, messageId);

            var handler = messageHandlerRegistry.GetHandler(envelope.MessageType);

            if (handler is null)
            {
                logger.LogWarning("No handler registered for message type {MessageType}", envelope.MessageType);
                // We still complete the message since we don't want to reprocess it if there's no handler
                await args.CompleteMessageAsync(args.Message);
                return;
            }

            await handler.HandleAsync(envelope.MessageContent, args.CancellationToken);
            await args.CompleteMessageAsync(args.Message);

            logger.LogInformation("Successfully processed message {MessageId}", messageId);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error processing message {MessageId}", messageId);
            await args.AbandonMessageAsync(args.Message);
        }
    }

    private Task ProcessErrorAsync(ProcessErrorEventArgs args)
    {
        logger.LogError(args.Exception, "Error in Service Bus processing. Error source: {ErrorSource}",
            args.ErrorSource);
        return Task.CompletedTask;
    }

    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        if (_processor is not null)
        {
            await _processor.StopProcessingAsync(cancellationToken);
            logger.LogInformation("Service bus processor stopped");
        }

        await base.StopAsync(cancellationToken);
    }

    public override void Dispose()
    {
        _processor?.DisposeAsync()
            .AsTask()
            .GetAwaiter()
            .GetResult();

        base.Dispose();
    }
}