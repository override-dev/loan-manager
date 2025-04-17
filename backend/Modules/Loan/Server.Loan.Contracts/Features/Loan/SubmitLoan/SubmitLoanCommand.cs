using Ardalis.Result;
using FastEndpoints;

namespace Server.Loan.Contracts.Features.Loan.SubmitLoan;

public class SubmitLoanCommand:ICommand<Result<SubmitLoanCommandResponse>>
{
    public int LoanAmount { get; set; }

    public int LoanTerm { get; set; }

    public int LoanPurpose { get; set; }

    public required BankInformationDto BankInformation { get; set; }

    public required PersonalInformationDto PersonalInformation { get; set; }
}


