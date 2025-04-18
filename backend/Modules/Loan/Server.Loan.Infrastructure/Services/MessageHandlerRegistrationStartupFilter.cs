using Loan.Shared.Contracts.Requests;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Server.Loan.Application.Interfaces;
using Server.Loan.Infrastructure.Services.Handlers;

namespace Server.Loan.Infrastructure.Services;

internal class MessageHandlerRegistrationStartupFilter(
    IMessageHandlerRegistry registry,
    IServiceProvider serviceProvider) : IStartupFilter
{
    public Action<IApplicationBuilder> Configure(Action<IApplicationBuilder> next)
    {
        // Register all message handlers
        registry.RegisterHandler(
            nameof(LoanSubmissionRequested),
            serviceProvider.GetRequiredService<SubmitLoanRequestHandler>());

        // Add more handlers as needed

        return next;
    }
}
