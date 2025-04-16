namespace Server.Loan.Domain.Entities
{
    internal record Loan(Guid Id, string Name, string Description, decimal Price, int StockQuantity, DateTime CreatedAt);
}
