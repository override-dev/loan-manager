using Bff.Interfaces;
using FastEndpoints;
using Loan.StorageProvider.Interfaces;
using Loan.StorageProvider.Models;

namespace Bff.EndPoints.Loan.Submit;


//TODO: apply outbox pattern 
internal class SubmitLoanEndPoint(IStorageProvider storageProvider, ILoanPublisher loanPublisher):Endpoint<SubmitLoanRequest, SubmitLoanResponse>
{
    public override void Configure()
    {
        Post("/");
        Group<LoanGroup>(); 
        AllowAnonymous();
        Summary(s =>
        {
            s.Summary = "Submit a loan application";
            s.Description = "Submit a loan application to the system";
            s.Response<SubmitLoanResponse>();
            s.Response(400, "Invalid request");
            s.Response(500, "Internal server error");
        });
    }
    public override async Task HandleAsync(SubmitLoanRequest req, CancellationToken ct)
    {
        //1.- Create the loan entity
        var loanEntity = new LoanEntity
        {
            LoanId = Guid.NewGuid().ToString(),
            LoanAmount = req.LoanAmount,
            LoanTerm = req.LoanTerm,
            LoanPurpose = req.LoanPurpose,
            BankInformation= new BankInformationEntity(req.BankInformation.BankName, req.BankInformation.AccountType, req.BankInformation.AccountNumber),
            PersonalInformation = new PersonalInformationEntity(req.PersonalInformation.FullName, req.PersonalInformation.Email, DateOnly.FromDateTime(req.PersonalInformation.DateOfBirth)),
        };

        //2.- Save the loan entity to the storage provider

        var loanCreationResult = await storageProvider.CreateLoanAsync(loanEntity);


        //3.- Publish the loan creation event to the message broker
        var loanCreationRequest = new global::Loan.Shared.Contracts.Commands.SubmitLoanRequest(
            loanCreationResult.LoanId,// we pass the loan ID from the storage provider
            req.LoanAmount,
            req.LoanTerm,
            req.LoanPurpose,
            new(req.BankInformation.BankName, req.BankInformation.AccountType, req.BankInformation.BankName),
            new(req.PersonalInformation.FullName, req.PersonalInformation.Email, req.PersonalInformation.DateOfBirth)
        );

        await loanPublisher.PublishLoanSubmittedAsync(loanCreationRequest, ct);

        //4.- Return the loan ID to the client
        await SendOkAsync(new SubmitLoanResponse(loanCreationResult.LoanId), cancellation: ct);
    }
}
