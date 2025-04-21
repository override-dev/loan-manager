using Ardalis.Result;
using Server.Loan.Domain.Constants;

namespace Server.Loan.Domain.Aggregates.Loan.Entities;

internal class PersonalInformation
{

    public string FullName { get; private set; }

    public string Email { get; private set; }

    public DateOnly DateOfBirth { get; private set; }

    private PersonalInformation(string fullName, string email, DateOnly dateOfBirth)
    {
        FullName = fullName;
        Email = email;
        DateOfBirth = dateOfBirth;
    }

    public static Result<PersonalInformation> Create(string fullName, string email, DateOnly dateOfBirth)
    {
        var personalInformation = new PersonalInformation(fullName, email, dateOfBirth);

        var validationResult = personalInformation.Validate();
        if (!validationResult.IsSuccess)
        {
            return validationResult.Map();
        }
        return Result.Success(personalInformation);
    }


    public Result Validate()
    {
        if (string.IsNullOrEmpty(FullName))
        {
            return Result.Invalid(new ValidationError(nameof(FullName), string.Empty, DomainErrors.PersonalInformation.FULL_NAME_REQUIRED, ValidationSeverity.Error));
        }
        if (string.IsNullOrEmpty(Email))
        {
            return Result.Invalid(new ValidationError(nameof(Email), string.Empty, DomainErrors.PersonalInformation.EMAIL_REQUIRED, ValidationSeverity.Error));
        }
        if (DateOfBirth == default)
        {
            return Result.Invalid(new ValidationError(nameof(DateOfBirth), string.Empty, DomainErrors.PersonalInformation.DATE_OF_BIRTH_REQUIRED, ValidationSeverity.Error));
        }
        return Result.Success();
    }
}
