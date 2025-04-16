using Server.Loan.Domain.Aggregates.Loan;
using Server.Loan.Domain.Interfaces;

internal class LoanService : ILoanService
{
    public async Task<Loan> CreateLoanAsync(string name, string description, decimal price, int stockQuantity)
    {
        var item = new Loan(Guid.NewGuid(), name, description, price, stockQuantity, DateTime.UtcNow);

        await Task.Delay(100);

        return item;
    }
}
