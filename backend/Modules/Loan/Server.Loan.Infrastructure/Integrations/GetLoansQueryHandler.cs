using Ardalis.Result;
using FastEndpoints;
using Server.Loan.Contracts.Features.Loan.GetLoan;
using Server.Loan.Infrastructure.Interfaces;

namespace Server.Loan.Infrastructure.Integrations;

internal class GetLoansQueryHandler(ILoanRepository loanRepository) : CommandHandler<GetLoansQuery, Result<GetLoansQueryResponse>>
{
    public override async Task<Result<GetLoansQueryResponse>> ExecuteAsync(GetLoansQuery command, CancellationToken ct = default)
    {
       var loans= await loanRepository.GetAllLoansAsync();

        var items= loans.Select(loan => new LoanDto(
            Id: loan.LoanId,
            LoanAmount: loan.LoanAmount,
            LoanTerm: loan.LoanTerm,
            LoanPurpose: loan.LoanPurpose,
            LoanStatus: (int)loan.LoanStatus,
            BankAccountNumber: loan.BankInformation.AccountNumber,
            BankAccountType: loan.BankInformation.AccountType,
            BankName: loan.BankInformation.BankName,
            FullName: loan.PersonalInformation.FullName,
            Email: loan.PersonalInformation.Email,
            DateOfBirth: loan.PersonalInformation.DateOfBirth.ToDateTime(TimeOnly.MinValue)
        )).ToList();

        return new GetLoansQueryResponse(items);
    }
}
