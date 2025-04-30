namespace Loan.Shared.Contract.Abstractions.Interfaces;

public interface ISchemaValidator
{
    Task<bool> ValidateAsync<T>(T message);
}
