using System.ComponentModel.DataAnnotations;
using Ardalis.Result;
using Server.Loan.Domain.Constants;
using static Server.Loan.Domain.Constants.DomainErrors;

namespace Server.Loan.Domain.Aggregates.Loan.Entities;

internal class BankInformation
{

    public string BankName { get; private set; }

    public string AccountType { get; private set; }

    public string AccountNumber { get; private set; }


    private BankInformation(string bankName, string accountType, string accountNumber)
    {
        BankName = bankName;
        AccountType = accountType;
        AccountNumber = accountNumber;
    }

    public static Result<BankInformation> Create(string bankName, string accountType, string accountNumber)
    {
        var bankInformation =  new BankInformation(bankName, accountType, accountNumber);

        var validationResult = bankInformation.Validate();
        if (!validationResult.IsSuccess)
        {
            return validationResult.Map();
        }
        return Result.Success(bankInformation);
    }


    public Result Validate()
    {
        if(string.IsNullOrEmpty(BankName))
        {
            return Result.Invalid(new ValidationError(nameof(BankName), string.Empty, DomainErrors.BankInformation.BANK_NAME_REQUIRED, ValidationSeverity.Error));
        }

        if (string.IsNullOrEmpty(AccountType))
        {
            return Result.Invalid(new ValidationError(nameof(AccountType), string.Empty, DomainErrors.BankInformation.ACCOUNT_TYPE_REQUIRED, ValidationSeverity.Error));
        }

        if (string.IsNullOrEmpty(AccountNumber))
        {
            return Result.Invalid(new ValidationError(nameof(AccountNumber), string.Empty, DomainErrors.BankInformation.ACCOUNT_NUMBER_REQUIRED, ValidationSeverity.Error));
        }

        return Result.Success();
    }

}
