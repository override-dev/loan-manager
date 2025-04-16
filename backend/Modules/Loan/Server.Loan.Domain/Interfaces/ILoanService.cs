namespace Server.Loan.Domain.Interfaces;

internal interface ILoanService
{
    Task<Aggregates.Loan.Loan> CreateLoanAsync(string name, string description, decimal price, int stockQuantity);
}
