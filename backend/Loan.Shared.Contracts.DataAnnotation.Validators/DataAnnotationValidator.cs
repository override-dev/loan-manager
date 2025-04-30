using Loan.Shared.Contract.Abstractions.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace Loan.Shared.Contracts.DataAnnotation.Validators;

public class DataAnnotationValidator : ISchemaValidator
{
    public Task<bool> ValidateAsync<T>(T message)
    {
        if(message is null)
        {
            return Task.FromResult(false);
        }

        var validationResults = new List<ValidationResult>();
        bool isValid = Validator.TryValidateObject(
            message,
            new ValidationContext(message),
            validationResults,
            validateAllProperties: true);

        return Task.FromResult(isValid);
    }
}
