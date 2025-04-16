namespace Server.Loan.EndPoints.Loan.Create
{
    public record CreateLoanResponse(Guid Id, string Name, string? Description, decimal Price, int StockQuantity, DateTime CreatedAt);
}
