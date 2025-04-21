using Ardalis.Result;
using Server.Loan.Domain.Aggregates.Loan.DomainEvents;
using Server.Loan.Domain.Aggregates.Loan.Entities;
using Server.Loan.Domain.Aggregates.Loan.Enums;
using Server.Loan.Domain.Aggregates.Loan.ValueObjects;
using Server.Loan.Domain.Constants;
using Server.Loan.Domain.Interfaces;

namespace Server.Loan.Domain.Aggregates.Loan;


internal class Loan
{

    public LoanId Id { get; private set; }

    public int LoanAmount { get; private set; }

    public int LoanTerm { get; private set; }

    public int LoanPurpose { get; private set; }

    public BankInformation BankInformation { get; private set; }

    public LoanStatus LoanStatus { get; private set; } = LoanStatus.Pending;

    public PersonalInformation PersonalInformation { get; private set; }

    private readonly List<IDomainEvent> _domainEvents = [];
    public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

    private Loan(LoanId Id, int LoanAmount, int LoanTerm, int LoanPurpose, LoanStatus loanStatus, PersonalInformation personalInformation, BankInformation bankInformation)
    {
        this.Id = Id;
        this.LoanAmount = LoanAmount;
        this.LoanTerm = LoanTerm;
        this.LoanPurpose = LoanPurpose;
        PersonalInformation = personalInformation;
        BankInformation = bankInformation;
        LoanStatus = loanStatus;
    }


    public static Result<Loan> Create(LoanId Id, int LoanAmount, int LoanTerm, int LoanPurpose, LoanStatus loanStatus, PersonalInformation personalInformation, BankInformation bankInformation)
    {
        var loan = new Loan(Id, LoanAmount, LoanTerm, LoanPurpose,loanStatus, personalInformation,bankInformation);

        var validationResult = loan.Validate();

        if (!validationResult.IsSuccess)
        {
            return validationResult.Map();
        }
        return Result.Success(loan);
    }


    public Result Approve()
    {
        if (LoanStatus != LoanStatus.Pending)
        {
            return Result.Invalid(new ValidationError(nameof(LoanStatus), string.Empty, DomainErrors.Loan.LOAN_STATUS_INVALID, ValidationSeverity.Error));
        }

        LoanStatus = LoanStatus.Approved;
        _domainEvents.Add(new LoanApprovedEvent(Id));

        return Result.Success();
    }

    public Result Cancel()
    {
        if (LoanStatus != LoanStatus.Pending)
        {
            return Result.Invalid(new ValidationError(nameof(LoanStatus), string.Empty, DomainErrors.Loan.LOAN_STATUS_INVALID, ValidationSeverity.Error));
        }

        LoanStatus = LoanStatus.Canceled;
        _domainEvents.Add(new LoanCanceledEvent(Id));

        return Result.Success();
    }

    public Result Reject()
    {
        if (LoanStatus != LoanStatus.Pending)
        {
            return Result.Invalid(new ValidationError(nameof(LoanStatus), string.Empty, DomainErrors.Loan.LOAN_STATUS_INVALID, ValidationSeverity.Error));
        }

        LoanStatus = LoanStatus.Rejected;
        _domainEvents.Add(new LoanRejectedEvent(Id));

        return Result.Success();
    }

    public Result Submit()
    {
        if (LoanStatus != LoanStatus.Pending)
        {
            return Result.Invalid(new ValidationError(nameof(LoanStatus), string.Empty, DomainErrors.Loan.LOAN_STATUS_INVALID, ValidationSeverity.Error));
        }

        LoanStatus = LoanStatus.Submitted;
        _domainEvents.Add(new LoanSubmittedEvent(Id));

        return Result.Success();
    }

    public Result CreateNewLoan()
    {
        if (LoanStatus != LoanStatus.Pending)
        {
            return Result.Invalid(new ValidationError(nameof(LoanStatus), string.Empty, DomainErrors.Loan.LOAN_STATUS_INVALID, ValidationSeverity.Error));
        }
        LoanStatus =  LoanStatus.Created;
        _domainEvents.Add(new LoanCreatedEvent(Id));
        return Result.Success();
    }

    public Result Reset()
    {
        LoanStatus = LoanStatus.Pending;
        _domainEvents.Add(new LoanResetEvent(Id));

        return Result.Success();
    }
    public void ClearDomainEvents()
    {
        _domainEvents.Clear();
    }

    public Result Validate()
    {
        if (Id == default)
        {
            return Result.Invalid(new ValidationError(nameof(Id), string.Empty, DomainErrors.Loan.LOAN_NOT_FOUND, ValidationSeverity.Error));
        }

        if (LoanAmount <= 0)
        {
            return Result.Invalid(new ValidationError(nameof(LoanAmount),string.Empty,DomainErrors.Loan.LOAN_AMOUNT_INVALID, ValidationSeverity.Error));
        }
        if (LoanTerm <= 0)
        {
            return Result.Invalid(new ValidationError(nameof(LoanTerm), string.Empty, DomainErrors.Loan.LOAN_TERM_INVALID, ValidationSeverity.Error));
        }
        if (LoanPurpose <= 0)
        {
            return Result.Invalid(new ValidationError(nameof(LoanPurpose),string.Empty,DomainErrors.Loan.LOAN_PURPOSE_INVALID,ValidationSeverity.Error));
        }

        var personalValidationResult = PersonalInformation.Validate();
        if (!personalValidationResult.IsSuccess)
        {
            return personalValidationResult.Map();
        }

        var bankValidationResult = BankInformation.Validate();
        if (!bankValidationResult.IsSuccess)
        {
            return bankValidationResult.Map();
        }
      
        return Result.Success();
    }
}
