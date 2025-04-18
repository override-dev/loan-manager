using Loan.Shared.Contracts.Abstractions;
using Loan.Shared.Contracts.Models;

namespace Loan.Shared.Contracts.Commands;

public record SubmitLoanRequest(string LoanId, int LoanAmount, int LoanTerm, int LoanPurpose, BankInformation BankInformation, PersonalInformation PersonalInformation) : BaseMessage;
