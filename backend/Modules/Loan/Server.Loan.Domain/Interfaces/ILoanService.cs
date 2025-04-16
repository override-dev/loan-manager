namespace Server.Loan.Domain.Interfaces
{
    internal interface ILoanService
    {
        Task<Entities.Loan> CreateProductAsync(string name, string description, decimal price, int stockQuantity);
    }
}
