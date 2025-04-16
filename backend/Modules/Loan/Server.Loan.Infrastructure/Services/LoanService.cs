

using Server.Loan.Domain.Entities;
using Server.Loan.Domain.Interfaces;

internal class LoanService : ILoanService
{
    public async Task<Loan> CreateProductAsync(string name, string description, decimal price, int stockQuantity)
    {
        var item = new Loan(Guid.NewGuid(), name, description, price, stockQuantity, DateTime.UtcNow);

        await Task.Delay(100);

        return item;
    }
}
