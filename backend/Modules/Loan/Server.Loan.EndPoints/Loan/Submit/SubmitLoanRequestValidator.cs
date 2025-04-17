using FluentValidation;

namespace Server.Loan.EndPoints.Loan.Submit;

internal class SubmitLoanRequestValidator:AbstractValidator<SubmitLoanRequest>
{
    public SubmitLoanRequestValidator()
    {
        RuleFor(x => x.LoanAmount)
            .NotEmpty()
            .WithMessage("Loan amount is required")
            .GreaterThan(0)
            .WithMessage("Loan amount must be greater than 0");
        RuleFor(x => x.LoanTerm)
            .NotEmpty()
            .WithMessage("Loan term is required")
            .GreaterThan(0)
            .WithMessage("Loan term must be greater than 0");
        RuleFor(x => x.LoanPurpose)
            .NotEmpty()
            .WithMessage("Loan purpose is required");
        RuleFor(x => x.BankInformation)
            .NotNull()
            .WithMessage("Bank information is required");
        RuleFor(x => x.PersonalInformation)
            .NotNull()
            .WithMessage("Personal information is required");
    }
}
